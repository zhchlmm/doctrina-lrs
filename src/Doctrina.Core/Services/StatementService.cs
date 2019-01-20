using Doctrina.Core.Data;
using Doctrina.Core.Models;
using Doctrina.Core.Repositories;
using Doctrina.xAPI;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctrina.Core.Services
{
    public class StatementService : StatementBaseService<StatementEntity>, IStatementService
    {
        private readonly IStatementRepository _statements;
        private readonly IVerbService _verbService;
        private readonly IAgentService _agentService;
        private readonly IActivityService _activityService;
        private readonly ISubStatementService _subStatementService;
        private readonly IAttachmentService _attachmentService;

        public StatementService(DoctrinaContext dbContext, IStatementRepository statementRepository, IVerbService verbService, IAgentService agentService, IActivityService activityService, ISubStatementService subStatementService, IAttachmentService attachmentService, ILogger<StatementService> logger)
            : base(dbContext, agentService, activityService, subStatementService, logger)
        {
            _statements = statementRepository;
            _verbService = verbService;
            _agentService = agentService;
            _activityService = activityService;
            _subStatementService = subStatementService;
            _attachmentService = attachmentService;
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
        /// Create a new Statement entity, without persisting.
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="statements">An array of Statements or a single Statement to be stored.</param>
        /// <returns>Array of Statement id(s) (UUID) in the same order as the corresponding stored Statements.</returns>
        public StatementEntity CreateStatement(Statement statement)
        {
            if (statement.Authority == null)
            {
                throw new ArgumentException(nameof(statement.Authority));
            }

            // Prevent conflic
            if (statement.Id.HasValue)
            {
                // TODO: Statement Comparision Requirements
                /// https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#statement-comparision-requirements
                var exist = GetStatement(statement.Id.Value);
                if (exist != null)
                    return exist;
            }

            var verb = _verbService.MergeVerb(statement.Verb);
            var actor = _agentService.MergeActor(statement.Actor);

            statement.Stamp();

            var entity = new StatementEntity()
            {
                StatementId = statement.Id.Value,
                ActorKey = actor.Key,
                Actor = actor,
                VerbKey = verb.Key,
                Verb = verb,
                Stored = DateTime.UtcNow,
                Timestamp = statement.Timestamp.Value,
                // TODO: Implement which store
            };

            entity.Version = statement.Version != null ? statement.Version.ToString() : ApiVersion.GetLatest().ToString();

            // TODO: HandleStatementAuthority(statement, model.Authority);
            MergeTarget(entity, statement.Object);
            MergeContext(entity, statement.Context);
            MergeResult(entity, statement.Result);
            MergeAuthority(entity, statement.Authority);

            if (statement.Attachments != null)
            {
                foreach (var attachment in statement.Attachments)
                {
                    _attachmentService.CreateAttachment(entity.StatementId, attachment);
                }
            }

            // Make sure the fullStatement is persistet
            statement.Id = entity.StatementId;
            statement.Stored = entity.Stored;
            statement.Timestamp = entity.Timestamp;
            statement.Version = entity.Version; // Update version if not applied

            // Check if this new statement, has been voided by another statement.
            entity.Voided = this._statements.HasVoidingStatement(statement.Id.Value);

            // Save the statement for quick parsing?
            entity.FullStatement = statement.ToJson();

            if (entity.Verb.Id == Verbs.Voided)
            {
                VoidStatement(entity);
            }

            _dbContext.Statements.Add(entity);

            return entity;
        }

        public IEnumerable<StatementEntity> GetStatements(PagedStatementsQuery parameters, out int totalCount)
        {
            totalCount = 0;
            bool includeAttachements = parameters.Attachments.GetValueOrDefault();

            // Exclude voided statements
            var query = _statements.AsQueryable(voided: false, includeAttachments: includeAttachements);

            // Limit results by stored date since timestamp
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

            if (parameters.ActivityId != null)
            {
                string strActivityId = parameters.ActivityId.ToString();

                query.Where(x => x.ObjectActivity.ActivityId == strActivityId);

                if (parameters.RelatedActivities.GetValueOrDefault())
                {
                    query.Where(x =>
                        x.Context.ContextActivities.Category.Any(ca => ca.ActivityId == strActivityId)
                    || x.Context.ContextActivities.Parent.Any(parent => parent.ActivityId == strActivityId)
                    || x.Context.ContextActivities.Grouping.Any(grouping => grouping.ActivityId == strActivityId)
                    || x.Context.ContextActivities.Other.Any(other => other.ActivityId == strActivityId)
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
            foreach (var item in pagedResult)
            {
                var stmt = item.Statement;
                if (includeAttachements)
                    stmt.Attachments = item.Attachments;
                result.Add(stmt);
            }

            return result;
        }

        public async Task SaveAsync(params StatementEntity[] statements)
        {
            foreach (var statement in statements)
            {
                _dbContext.Statements.Add(statement);
            }

            await _dbContext.SaveChangesAsync();
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

        public DateTimeOffset GetConsistentThroughDate()
        {
            var date = _dbContext.Statements
                .OrderByDescending(x => x.Stored)
                .Select(x => x.Stored)
                .FirstOrDefault();

            return date;
        }
    }
}
