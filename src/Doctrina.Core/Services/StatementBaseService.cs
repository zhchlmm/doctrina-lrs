using Doctrina.Core.Data;
using Doctrina.Core.Repositories;
using Doctrina.xAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Core.Services
{
    public abstract class StatementBaseService<TStatement>
        where TStatement : IStatementEntityBase
    {
        public readonly DoctrinaContext _dbContext;
        private readonly IAgentService _agentService;
        private readonly IActivityService _activityService;
        private readonly ISubStatementService _subStatementService;
        private readonly ILogger _logger;

        public StatementBaseService(DoctrinaContext dbContext, IAgentService agentService, IActivityService activityService, ISubStatementService subStatementService, ILogger logger)
        {
            _dbContext = dbContext;
            _agentService = agentService;
            _activityService = activityService;
            _subStatementService = subStatementService;
            _logger = logger;
        }

        /// <summary>
        /// Used to create instance without SubStatement 
        /// </summary>
        /// <param name="statementRepository"></param>
        /// <param name="agentService"></param>
        /// <param name="activityService"></param>
        public StatementBaseService(DoctrinaContext dbContext, IStatementRepository statementRepository, IAgentService agentService, IActivityService activityService)
        {
            _dbContext = dbContext;
            _agentService = agentService;
            _activityService = activityService;
        }

        public void MergeContext(TStatement stmt, Context context)
        {
            if (context == null)
                return;

            stmt.Context = new ContextEntity
            {
                ContextId = Guid.NewGuid()
            };

            if (context.Instructor != null)
            {
                var instructor = this._agentService.MergeActor(context.Instructor);
                stmt.Context.InstructorId = instructor.Key;
            }

            if (!string.IsNullOrEmpty(context.Language))
                stmt.Context.Language = context.Language;

            if (!string.IsNullOrEmpty(context.Platform))
                stmt.Context.Platform = context.Platform;

            if (context.Registration.HasValue)
                stmt.Context.Registration = context.Registration.Value;

            if (!string.IsNullOrEmpty(context.Revision))
                stmt.Context.Revision = context.Revision;

            if (context.Statement != null)
            {
                // TODO: Validate the statement's context statement reference?
                stmt.Context.StatementId = context.Statement.Id;
            }

            if (context.Team != null)
            {
                var agent = this._agentService.MergeActor(context.Team);
                stmt.Context.TeamId = agent.Key;
            }

            if (context.Extensions != null)
                stmt.Context.Extensions = context.Extensions.ToString();

            if(context.ContextActivities != null)
            {
                stmt.Context.ContextActivities = new ContextActivitiesEntity() {
                    Key = Guid.NewGuid()
                };
                // TODO: Validate there are any filled before setting key.
                if(context.ContextActivities.Category != null)
                {
                    stmt.Context.ContextActivities.Category = new List<ContextActivitiesCategory>();
                    foreach (var ent in context.ContextActivities.Category)
                        stmt.Context.ContextActivities.Category.Add(new ContextActivitiesCategory()
                        {
                            ActivityId = ent.Id.ToString()
                        });
                }

                if (context.ContextActivities.Parent != null)
                {
                    stmt.Context.ContextActivities.Parent = new List<ContextActivitiesParent>();
                    foreach (var ent in context.ContextActivities.Parent)
                        stmt.Context.ContextActivities.Parent.Add(new ContextActivitiesParent()
                        {
                            ActivityId = ent.Id.ToString()
                        });
                }

                if (context.ContextActivities.Grouping != null)
                {
                    stmt.Context.ContextActivities.Grouping = new List<ContextActivitiesGrouping>();
                    foreach (var ent in context.ContextActivities.Grouping)
                        stmt.Context.ContextActivities.Grouping.Add(new ContextActivitiesGrouping()
                        {
                            ActivityId = ent.Id.ToString()
                        });
                }

                if (context.ContextActivities.Other != null)
                {
                    stmt.Context.ContextActivities.Other = new List<ContextActivitiesOther>();
                    foreach (var ent in context.ContextActivities.Other)
                        stmt.Context.ContextActivities.Other.Add(new ContextActivitiesOther()
                        {
                            ActivityId = ent.Id.ToString()
                        });
                }
            }
        }

        public void MergeResult(TStatement stmt, Result result)
        {
            if (result == null)
                return;

            stmt.Result = new ResultEntity
            {
                Completion = result.Completion,
                Success = result.Success
            };

            if(result.Duration != null)
            {
                stmt.Result.Duration = result.Duration.Ticks;
            }

            if (!string.IsNullOrEmpty(result.Response))
                stmt.Result.Response = result.Response;

            if (result.Score != null)
            {
                var score = result.Score;

                if (score.Raw < score.Min)
                {
                    throw new InvalidOperationException($"Result 'Raw' cannot be less than 'Min'.");
                }

                if (score.Raw > score.Max)
                {
                    throw new InvalidOperationException($"Result 'Raw' cannot be greather than 'Max'.");
                }

                if (score.Min > score.Max)
                {
                    throw new InvalidOperationException($"Result 'Min' cannot be greather than 'Max'.");
                }

                stmt.Result.ScoreMax = result.Score.Max;
                stmt.Result.ScoreMin = result.Score.Min;
                stmt.Result.ScoreRaw = result.Score.Raw;
                stmt.Result.ScoreScaled = result.Score.Scaled;
            }
        }

        public virtual void MergeTarget(IStatementEntityBase stmt, StatementTargetBase target)
        {
            if (target == null)
                return;

            switch (target.ObjectType)
            {
                case ObjectType.Group:
                case ObjectType.Agent:
                    var agent = this._agentService.MergeActor((Agent)target);
                    stmt.ObjectAgentKey = agent.Key;
                    break;

                case ObjectType.Activity:
                    ActivityEntity activity = this._activityService.MergeActivity((Activity)target);
                    stmt.ObjectActivityKey = activity.Key;
                    break;

                case ObjectType.SubStatement:
                    SubStatementEntity subStatement = this._subStatementService.CreateSubStatement((SubStatement)target);
                    ((StatementEntity)stmt).ObjectSubStatementId = subStatement.Id;
                    break;

                case ObjectType.StatementRef:
                    StatementRef statementRef = ((StatementRef)target);
                    stmt.ObjectStatementRefId = statementRef.Id;

                    // Void statement?
                    if (stmt.Verb.Id == Verbs.Voided && stmt.ObjectStatementRefId.HasValue)
                    {
                       VoidStatement(stmt);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stmt">Statement to be voided</param>
        // https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#requirements-1
        public void VoidStatement(IStatementEntityBase voidingStatement)
        {
            // When issuing a Statement that voids another, the Object of that voiding Statement MUST have the "objectType" property set to StatementRef.
            if (!voidingStatement.ObjectStatementRefId.HasValue)
                throw new Exception("When issuing a Statement that voids another, the Object of that voiding Statement MUST have the 'objectType' property set to StatementRef.");

            // An LRS MUST consider a Statement it contains voided if and only if the Statement is not itself a voiding Statement and the LRS also contains a voiding Statement referring to the first Statement.
            if (voidingStatement.Verb.Id != Verbs.Voided)
                throw new Exception("Any Statement that voids another must have the verb: \"" + Verbs.Voided + "\"");

            var statementRefId = voidingStatement.ObjectStatementRefId.Value;
            var voidedStatement = this._dbContext.Statements
                .Include(x=> x.Verb)
                .FirstOrDefault(x=> x.StatementId == statementRefId);

            // Upon receiving a Statement that voids another, the LRS SHOULD NOT* reject the request on the grounds of the Object of that voiding Statement not being present.
            if (voidedStatement == null)
                return;

            // Any Statement that voids another cannot itself be voided.
            if (voidedStatement.Verb.Id == Verbs.Voided)
                return;

            // voidedStatement has been voided, return.
            if (voidedStatement.Voided)
                return;

            voidedStatement.Voided = true;

            this._dbContext.Statements.Update(voidedStatement);
            this._dbContext.Entry(voidedStatement).State = EntityState.Modified;
        }
    }
}
