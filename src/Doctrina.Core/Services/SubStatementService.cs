using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Repositories;
using System;
using xAPI.Core.Models;

namespace Doctrina.Core.Services
{
    sealed class SubStatementService : StatementBaseService<SubStatementEntity>, ISubStatementService
    {
        private readonly ISubStatementRepository subStatements;
        private readonly IVerbService verbService;
        private readonly IAgentService agentService;

        public SubStatementService(ISubStatementRepository subStatementRepository, StatementRepository statementRepository, IVerbService verbService, IAgentService agentService, IActivityService activityService)
            : base(statementRepository, agentService, activityService)
        {
            this.subStatements = subStatementRepository;
            this.verbService = verbService;
            this.agentService = agentService;
        }

        public SubStatementEntity CreateSubStatement(SubStatement model)
        {
            this.verbService.MergeVerb(model.Verb);
            var actor = this.agentService.MergeAgent(model.Actor);

            var entity = new SubStatementEntity()
            {
                ActorId = actor.Id,
                VerbId = model.Verb.Id.ToString(),
            };

            MergeTarget(entity, model.Target);
            MergeContext(entity, model.Context);
            MergeResult(entity, model.Result);
            try
            {
                this.subStatements.Create(entity);
            }
            catch (Exception e)
            {
                //LogHelper.Error<SubStatementService>("SaveSubStatement: \r\n {0}", new Exception(model.ToJson()));
                throw e;
            }

            return entity;
        }

        public override void MergeTarget(IStatementBase stmt, StatementTargetBase target)
        {
            if (target == null)
                return;

            ObjectType objectType = target.ObjectType;
            if (objectType == ObjectType.SubStatement)
            {
                throw new NotImplementedException("SubStatements are not allowed as objectType of an SubStatement.");
            }

            base.MergeTarget(stmt, target);
        }
    }
}