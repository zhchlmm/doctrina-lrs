using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public abstract class StatementBaseService<TStatement>
        where TStatement : IStatementBase
    {
        private readonly IStatementRepository statements;
        private readonly IAgentService agentService;
        private readonly IActivityService activityService;
        private readonly ISubStatementService subStatementService;

        public StatementBaseService(IStatementRepository statementRepository,IAgentService agentService, IActivityService activityService, ISubStatementService subStatementService)
        {
            this.statements = statementRepository;
            this.agentService = agentService;
            this.activityService = activityService;
            this.subStatementService = subStatementService;
        }

        /// <summary>
        /// Used to create instance without SubStatement 
        /// </summary>
        /// <param name="statementRepository"></param>
        /// <param name="agentService"></param>
        /// <param name="activityService"></param>
        public StatementBaseService(IStatementRepository statementRepository, IAgentService agentService, IActivityService activityService)
        {
            this.statements = statementRepository;
            this.agentService = agentService;
            this.activityService = activityService;
        }

        public void MergeContext(TStatement stmt, Context context)
        {
            if (context == null)
                return;

            stmt.Context = new ContextEntity();

            if (context.Instructor != null)
            {
                var instructor = this.agentService.MergeAgent(context.Instructor);
                stmt.Context.InstructorId = instructor.Id;
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
                var agent = this.agentService.MergeAgent(context.Team);
                stmt.Context.TeamId = agent.Id;
            }

            if (context.Extensions != null)
                stmt.Context.Extensions = context.Extensions.ToString();
        }

        public void MergeResult(TStatement stmt, Result result)
        {
            if (result == null)
                return;

            stmt.Result = new ResultEntity
            {
                Completion = result.Completion,
                Success = result.Success,
                Duration = result.Duration
            };

            if (!string.IsNullOrEmpty(result.Response))
                stmt.Result.Response = result.Response;

            if (result.Score != null)
            {
                stmt.Result.ScoreMax = result.Score.Max;
                stmt.Result.ScoreMin = result.Score.Min;
                stmt.Result.ScoreRaw = result.Score.Raw;
                stmt.Result.ScoreScaled = result.Score.Scaled;
            }
        }

        public virtual void MergeTarget(IStatementBase stmt, StatementTargetBase target)
        {
            if (target == null)
                return;

            switch (target.ObjectType)
            {
                case ObjectType.Group:
                case ObjectType.Agent:
                    var agent = this.agentService.MergeAgent((Agent)target);
                    stmt.ObjectAgentId = agent.Id;
                    break;

                case ObjectType.Activity:
                    ActivityEntity activity = this.activityService.MergeActivity((Activity)target);
                    stmt.ObjectActivityId = activity.ActivityId;
                    break;

                case ObjectType.SubStatement:
                    SubStatementEntity subStatement = this.subStatementService.CreateSubStatement((SubStatement)target);
                    ((StatementEntity)stmt).ObjectSubStatementId = subStatement.Id;
                    break;

                case ObjectType.StatementRef:
                    StatementRef statementRef = ((StatementRef)target);
                    stmt.ObjectStatementRefId = statementRef.Id;

                    // Void statement?
                    if (stmt.VerbId == Verbs.Voided && stmt.ObjectStatementRefId.HasValue)
                    {
                        try
                        {
                            VoidStatement(stmt);
                        }
                        catch (Exception e)
                        {
                            //LogHelper.Info<StatementService>(() => e.Message);
                        }
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
        public void VoidStatement(IStatementBase voidingStatement)
        {
            // When issuing a Statement that voids another, the Object of that voiding Statement MUST have the "objectType" property set to StatementRef.
            if (!voidingStatement.ObjectStatementRefId.HasValue)
                throw new Exception("When issuing a Statement that voids another, the Object of that voiding Statement MUST have the 'objectType' property set to StatementRef.");

            // An LRS MUST consider a Statement it contains voided if and only if the Statement is not itself a voiding Statement and the LRS also contains a voiding Statement referring to the first Statement.
            if (voidingStatement.VerbId != Verbs.Voided)
                throw new Exception("Any Statement that voids another must have the verb: \"" + Verbs.Voided + "\"");

            var statementRefId = voidingStatement.ObjectStatementRefId.Value;
            var voidedStatement = this.statements.GetById(statementRefId);

            // Upon receiving a Statement that voids another, the LRS SHOULD NOT* reject the request on the grounds of the Object of that voiding Statement not being present.
            if (voidedStatement == null)
                return;

            // Any Statement that voids another cannot itself be voided.
            if (voidedStatement.VerbId == Verbs.Voided)
                return;

            // voidedStatement has been voided, return.
            if (voidedStatement.Voided)
                return;

            voidedStatement.Voided = true;

            this.statements.Update(voidedStatement);
        }
    }
}
