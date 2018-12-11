using Doctrina.Core.Models;
using Doctrina.Core.Data;
using Doctrina.Core.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.Extensions.Logging;
using Doctrina.xAPI.Http;

namespace Doctrina.Core.Services
{
    public class StatementService : StatementBaseService<StatementEntity>, IStatementService
    {
        private readonly IStatementRepository _statements;
        private readonly IVerbService _verbService;
        private readonly IAgentService _agentService;
        private readonly IActivityService _activityService;
        private readonly ISubStatementService _subStatementService;

        public StatementService(DoctrinaContext dbContext, IStatementRepository statementRepository, IVerbService verbService, IAgentService agentService, IActivityService activityService, ISubStatementService subStatementService, ILogger<StatementService> logger)
            : base(dbContext, agentService, activityService, subStatementService, logger)
        {
            _statements = statementRepository;
            _verbService = verbService;
            _agentService = agentService;
            _activityService = activityService;
            _subStatementService = subStatementService;
        }

        public StatementEntity GetStatement(Guid statementId, bool voided = false, bool includeAttachments = false)
        {
            var stmt = this._statements.GetById(statementId, voided, includeAttachments);
            if (stmt == null)
                return null;

            // Protect against return a statement where voided state is not equal the voided param
            if (stmt.Voided != voided)
                return null;

            return stmt;
        }

        /// <summary>
        /// Stores a Statement, or a set of Statements.
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="statements">An array of Statements or a single Statement to be stored.</param>
        /// <returns>Array of Statement id(s) (UUID) in the same order as the corresponding stored Statements.</returns>
        public Guid[] CreateStatements(Agent authority, params Statement[] statements)
        {
            var guids = new List<Guid>();
            foreach (var statement in statements)
            {
                if (statement.Authority == null)
                {
                    statement.Authority = authority;
                }
                else
                {
                    // TODO: Validate authority
                    //throw new NotImplementedException();
                }
                guids.Add(CreateStatement(statement)); // Now it does have a value
            }

            // Now save all the statements in a single commit

            this._dbContext.SaveChanges();

            return guids.ToArray();
        }

        private Guid CreateStatement(Statement model)
        {
            // Prevent conflic
            if (model.Id.HasValue)
            {
                // TODO: Statement Comparision Requirements
                /// https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#statement-comparision-requirements
                if (Exist(model.Id.Value))
                {
                    return model.Id.Value;
                }
            }

            var verb = this._verbService.MergeVerb(model.Verb);
            var actor = this._agentService.MergeActor(model.Actor);

            model.Stamp();

            var entity = new StatementEntity()
            {
                StatementId = model.Id.Value,
                ActorKey = actor.Key,
                Actor = actor,
                VerbKey = verb.Key,
                Verb = verb,
                Stored = DateTime.UtcNow,
                Timestamp = model.Timestamp.Value,
                //User = Guid.NewGuid() // Default to Umbraco Master
            };

            entity.Version = model.Version != null ? model.Version.ToString() : XAPIVersion.Latest().ToString();

            // TODO: HandleStatementAuthority(statement, model.Authority);
            MergeTarget(entity, model.Object);
            MergeContext(entity, model.Context);
            MergeResult(entity, model.Result);
            MergeAuthority(entity, model.Authority);
            /// Attachments are handled by the <see ref="AttachmentService">.

            // Make sure the fullStatement is persistet
            model.Id = entity.StatementId;
            model.Stored = entity.Stored;
            model.Timestamp = entity.Timestamp;
            model.Version = entity.Version; // Update version if not applied

            // Check if this new statement, has been voided by another statement.
            entity.Voided = this._statements.HasVoidingStatement(model.Id.Value);

            // TODO: Save the statement for quick parsing, or perhaps just cache the statements?
            entity.FullStatement = model.ToJson();

            if (entity.Verb.Id == Verbs.Voided)
            {
                VoidStatement(entity);
            }

            this._statements.AddStatement(entity);

            return model.Id.Value;
        }

        public IEnumerable<StatementEntity> GetStatements(PagedStatementsQuery parameters, out int totalCount)
        {
            totalCount = 0;
            bool includeAttachements = parameters.Attachments.GetValueOrDefault();

            // Exclude voided statements
            var query = _statements.GetAll(voided: false, includeAttachments: includeAttachements);

            // Limit results by stored date
            if (parameters.Since.HasValue)
            {
                DateTime since = parameters.Since.Value;
                query.Where(x => x.Stored >= since);
            }
            if (parameters.Until.HasValue)
            {
                DateTime until = parameters.Until.Value;
                query.Where(x => x.Stored <= until);
            }

            // For statements/read/mine oauth scope
            if (parameters.Agent != null)
            {
                // https://github.com/adlnet/ADL_LRS/blob/master/lrs/utils/retrieve_statement.py#L53
                if (parameters.RelatedAgents.HasValue && parameters.RelatedAgents.Value)
                {
                    throw new NotImplementedException();
                }
            }

            if (parameters.VerbId != null)
            {
                query.Where(x => x.Verb.Id == parameters.VerbId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(parameters.ActivityId))
            {
                if (parameters.RelatedActivities.GetValueOrDefault())
                {
                    query.Where(x =>
                        x.Context.ContextActivities.Category.Any(ca => ca.ActivityId == parameters.ActivityId)
                    || x.Context.ContextActivities.Parent.Any(parent => parent.ActivityId == parameters.ActivityId)
                    || x.Context.ContextActivities.Grouping.Any(grouping => grouping.ActivityId == parameters.ActivityId)
                    || x.Context.ContextActivities.Other.Any(other => other.ActivityId == parameters.ActivityId)
                    );
                }
            }

            if (parameters.Registration.HasValue)
            {
                query.Where(s => s.Context.Registration == parameters.Registration);
            }

            // Sort results acending?
            if (parameters.Ascending.HasValue)
            {
                query.OrderBy(x => x.Stored);
            }
            else
            {
                query.OrderByDescending(x => x.Stored);
            }


            // Ensure limit is not less than 0 or greather then MAX
            int limit = parameters.Limit.GetValueOrDefault(0);
            limit = Math.Max(limit, 0);
            //limit = Math.Min(limit, 1000);

            // Ensure skip is not less than 
            int skip = parameters.Skip.GetValueOrDefault(0);
            skip = Math.Max(skip, 0);
            parameters.Skip = skip; // Return skips

            var result = new List<StatementEntity>();

            var pageQuery = query.Select(x => new
            {
                TotalCount = query.Count(),
                Statement = x,
                Attachments = includeAttachements ? x.Attachments : new List<AttachmentEntity>()
            });

            if (limit > 0)
            {
                pageQuery = pageQuery.Skip(skip)
                .Take(limit);
            }

            // Execute query
            var pagedResult = pageQuery.ToList();

            if (!pagedResult.Any())
            {
                totalCount = 0;
                return new List<StatementEntity>();
            }

            // Push out total rows count
            totalCount = pagedResult.First().TotalCount;

            // Parse page result back to statements
            foreach(var item in pagedResult)
            {
                var stmt = item.Statement;
                if (includeAttachements)
                    stmt.Attachments = item.Attachments;
                result.Add(stmt);
            }

            return result;
        }

        public void SaveAndPublishWithStatus(StatementEntity statement)
        {
            this._statements.AddStatement(statement);
        }

        private void MergeAuthority(StatementEntity statement, Agent authority)
        {
            if (authority == null)
                throw new NullReferenceException("authority");

            ObjectType objType = authority.ObjectType;

            if (authority.ObjectType == ObjectType.Agent)
            {
                var agent = this._agentService.MergeActor(authority);
                statement.AuthorityId = agent.Key;
            }
            else if (authority.ObjectType == ObjectType.Group)
            {
                var group = authority as Group;
                // The two Agents represent an application and user together.
                if (group.Member.Count() != 2)
                    throw new JsonSerializationException("Group must contains exactly two Agents.");

                if (group.IsIdentified())
                    throw new JsonSerializationException("Identified group is not allowed");

                if (group.IsAnonymous())
                {

                }

                var agent = this._agentService.MergeActor(group);
                statement.AuthorityId = agent.Key;
                statement.Authority = agent;
            }
            else
            {
                throw new JsonSerializationException($"'{objType}' is not allowed as authority.");
            }
        }

        public Statement ConvertFrom(StatementEntity entity)
        {
            return JsonConvert.DeserializeObject<Statement>(entity.FullStatement);
        }

        public bool Exist(Guid statementId, bool voided = false)
        {
            return _statements.Exist(statementId, voided);
        }
    }
}
