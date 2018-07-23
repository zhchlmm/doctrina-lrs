using Doctrina.Core.Models;
using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public class StatementService : StatementBaseService<StatementEntity>, IStatementService
    {
        private readonly DoctrinaDbContext dbContext;
        private readonly IStatementRepository statements;
        private readonly IVerbService verbService;
        private readonly IAgentService agentService;
        private readonly IActivityService activityService;
        private readonly ISubStatementService subStatementService;

        public StatementService(DoctrinaDbContext dbContext, IStatementRepository statementRepository, IVerbService verbService, IAgentService agentService, IActivityService activityService, ISubStatementService subStatementService)
            : base (statementRepository, agentService, activityService, subStatementService)
        {
            this.dbContext = dbContext;
            this.statements = statementRepository;
            this.verbService = verbService;
            this.agentService = agentService;
            this.activityService = activityService;
            this.subStatementService = subStatementService;
        }

        public Statement GetStatement(Guid statementId, bool voided = false)
        {
            var stmt = this.statements.GetById(statementId);
            if (stmt == null)
                return null;

            // Protect against return a statement where voided state is not equal the voided param
            if (stmt.Voided != voided)
                return null;

            return ConvertFrom(stmt);
        }

        /// <summary>
        /// Stores a Statement, or a set of Statements.
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="statements">An array of Statements or a single Statement to be stored.</param>
        /// <returns>Array of Statement id(s) (UUID) in the same order as the corresponding stored Statements.</returns>
        public Guid[] SaveStatements(params Statement[] statements)
        {
            var guids = new List<Guid>();
            foreach (var statement in statements)
            {
                guids.Add(SaveStatement(statement)); // Now it does have a value
            }

            // Now save all the statements in a single commit

            this.dbContext.SaveChanges();

            return guids.ToArray();
        }

        public Guid SaveStatement(Statement model)
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

            this.verbService.MergeVerb(model.Verb);
            var actor = this.agentService.MergeAgent(model.Actor);

            model.Stamp();

            var entity = new StatementEntity()
            {
                ActorId = actor.Id,
                StatementId = model.Id.Value,
                VerbId = model.Verb.Id.ToString(),
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

            // Make sure the fullStatement is persistet
            model.Id = entity.StatementId;
            model.Stored = entity.Stored;
            model.Timestamp = entity.Timestamp;
            model.Version = entity.Version; // Update version if not applied

            // Check if this new statement, has been voided by another statement.
            entity.Voided = this.statements.HasVoidingStatement(model.Id.Value);

            // TODO: Save the statement for quick parsing, or perhaps just cache the statements?
            entity.FullStatement = model.ToJson();

            if (entity.VerbId == Verbs.Voided)
            {
                VoidStatement(entity);
            }

            this.statements.Save(entity);

            return model.Id.Value;
        }

        public PagedResult<StatementEntity> GetStatements(StatementsQuery parameters)
        {
            // Exclude voided statements
            var query = statements.GetAll(voided: false);

            // Sort results acending?
            if (parameters.Ascending.HasValue)
                query.OrderBy(x => x.Stored);
            else
                query.OrderByDescending(x => x.Stored);

            // Limit results by stored date
            if (parameters.Since.HasValue)
            {
                var since = parameters.Since.Value;
                query.Where(x => x.Stored >= since);
            }
            if (parameters.Until.HasValue)
            {
                var until = parameters.Until.Value;
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
                query.Where(x => x.VerbId == parameters.VerbId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(parameters.ActivityId))
            {
                if (parameters.RelatedActivities.GetValueOrDefault())
                {
                    query.Where(x => 
                        x.Context.ContextActivities.Category.Any(ca => ca.ActivityId == parameters.ActivityId)
                    ||  x.Context.ContextActivities.Parent.Any(parent => parent.ActivityId == parameters.ActivityId)
                    ||  x.Context.ContextActivities.Grouping.Any(grouping => grouping.ActivityId == parameters.ActivityId)
                    ||  x.Context.ContextActivities.Other.Any(other => other.ActivityId == parameters.ActivityId)
                    );
                }
            }

            if (parameters.Registration.HasValue)
            {
                query.Where(s => s.Context.Registration == parameters.Registration);
            }

            bool includeAttachements = parameters.Attachments.GetValueOrDefault();
            if (includeAttachements)
            {
                // TODO: Include attachements
            }


            int pageNumber = parameters.Page.HasValue ? parameters.Page.Value : 1;
            parameters.Page = pageNumber; // Set page

            int limit = parameters.Limit.GetValueOrDefault();
            limit = limit == 0 ? 1000 : limit;
            limit = Math.Min(limit, 1000); // Cap limit at 1000

            int pageIndex = Math.Max(0, pageNumber - 1);
            int skip = limit * pageIndex;

            int totalItemsCount = query.Count();
            var currentPageItems = query.Skip(skip).Take(limit).ToList();
            var pagedResult = new PagedResult<StatementEntity>(totalItemsCount, pageNumber, limit)
            {
                Items = currentPageItems
            };
            return pagedResult;
        }

        private void MergeAuthority(StatementEntity statement, Agent authority)
        {
            if (authority == null)
                throw new NullReferenceException("authority");

            var agent = this.agentService.MergeAgent(authority);
            statement.AuthorityId = agent.Id;
        }

        private Statement ConvertFrom(StatementEntity entity)
        {
            return JsonConvert.DeserializeObject<Statement>(entity.FullStatement);
        }

        //public IEnumerable<StatementEntity> GetStatements()
        //{
        //    return this.statements.GetAll(voided: false).ToList();
        //}

        public bool Exist(Guid statementId, bool voided = false)
        {
            return statements.Exist(statementId, voided);
        }
    }
}
