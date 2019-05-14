using Doctrina.Domain.Entities;
using Doctrina.Persistence.Repositories;
using Doctrina.xAPI;
using Doctrina.xAPI.Exceptions;
using Doctrina.xAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Persistence.Services
{
    public abstract class StatementBaseService<TStatement>
        where TStatement : StatementBaseEntity
    {
        public readonly DoctrinaDbContext _dbContext;
        private readonly IAgentService _agentService;
        private readonly IActivityService _activityService;
        private readonly ISubStatementService _subStatementService;
        private readonly ILogger _logger;

        public StatementBaseService(DoctrinaDbContext dbContext, IAgentService agentService, IActivityService activityService, ISubStatementService subStatementService, ILogger logger)
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
        public StatementBaseService(DoctrinaDbContext dbContext, IStatementRepository statementRepository, IAgentService agentService, IActivityService activityService)
        {
            _dbContext = dbContext;
            _agentService = agentService;
            _activityService = activityService;
        }


        public virtual void MergeTarget(StatementBaseEntity stmt, StatementObjectBase target)
        {
            if (target == null)
                return;

            switch (target.ObjectType)
            {
                case ObjectType.Group:
                case ObjectType.Agent:
                    stmt.ObjectAgent = _agentService.MergeActor((Agent)target);
                    break;

                case ObjectType.Activity:
                    stmt.ObjectActivity = _activityService.MergeActivity((Activity)target);
                    break;

                case ObjectType.SubStatement:
                    ((StatementEntity)stmt).ObjectSubStatement = _subStatementService.CreateSubStatement((SubStatement)target);
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

       
    }
}
