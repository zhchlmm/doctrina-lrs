using AutoMapper;
using Doctrina.Application.Agents.Commands;
using Doctrina.Application.Interfaces;
using Doctrina.Application.Statements.Commands;
using Doctrina.Application.Statements.Models;
using Doctrina.Application.Statements.Queries;
using Doctrina.Application.Verbs.Commands;
using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Extensions;
using Doctrina.xAPI;
using Doctrina.xAPI.Json;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Statements
{
    public class StatementsHandler :
        IRequestHandler<PagedStatementsQuery, PagedStatementsResult>,
        IRequestHandler<GetStatementQuery, Statement>,
        IRequestHandler<GetConsistentThroughQuery, DateTimeOffset?>,
        IRequestHandler<GetVoidedStatemetQuery, Statement>,
        IRequestHandler<CreateStatementCommand, Guid>,
        IRequestHandler<CreateStatementsCommand, ICollection<Guid>>,
        IRequestHandler<PutStatementCommand>,
        IRequestHandler<VoidStatementCommand>
    {
        private readonly IDoctrinaDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;

        public StatementsHandler(IDoctrinaDbContext context, IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<PagedStatementsResult> Handle(PagedStatementsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Statements.AsNoTracking();

            if (request.VerbId != null)
            {
                string verbHash = request.VerbId.ComputeHash();
                query = query.Where(x => x.Verb.Hash == verbHash);
            }

            if (request.Agent != null)
            {
                var actor = _mapper.Map<AgentEntity>(request.Agent);
                var currentAgent = await _context.Agents.WhereAgent(x => x, actor).FirstOrDefaultAsync(cancellationToken);
                if (currentAgent != null)
                {
                    Guid agentId = currentAgent.AgentId;
                    if (request.RelatedAgents.GetValueOrDefault())
                    {
                        query = (
                            from statement in query
                            where statement.Actor.AgentId == agentId
                            || (
                                statement.Object.ObjectType == EntityObjectType.Agent &&
                                statement.Object.Agent.AgentId == agentId
                            ) || (
                                statement.Object.ObjectType == EntityObjectType.SubStatement &&
                                (
                                    statement.Object.SubStatement.Actor.AgentId == agentId ||
                                    statement.Object.SubStatement.Object.ObjectType == EntityObjectType.Agent &&
                                    statement.Object.SubStatement.Object.Agent.AgentId == agentId
                                )
                            )
                            select statement);
                    }
                    else
                    {
                        query = query.WhereAgent(x => x.Actor, actor);
                    }
                }
                else
                {
                    return new PagedStatementsResult();
                }
            }

            if (request.ActivityId != null)
            {
                string activityHash = request.ActivityId.ComputeHash();

                if (request.RelatedActivities.GetValueOrDefault())
                {
                    query = (
                        from statement in query
                        where (
                            statement.Object.ObjectType == EntityObjectType.SubStatement && (
                                statement.Object.SubStatement.Object.ObjectType == EntityObjectType.Activity &&
                                statement.Object.SubStatement.Object.Activity.Hash == activityHash
                            ) ||
                            (
                                statement.Context != null && statement.Context.ContextActivities != null &&
                                (
                                    statement.Context.ContextActivities.Category.Contains(activityHash) ||
                                    statement.Context.ContextActivities.Parent.Contains(activityHash) ||
                                    statement.Context.ContextActivities.Grouping.Contains(activityHash) ||
                                    statement.Context.ContextActivities.Other.Contains(activityHash)
                                )
                            ) ||
                            (
                                statement.Object.ObjectType == EntityObjectType.Activity &&
                                statement.Object.Activity.Hash == activityHash
                            )
                        )
                        select statement
                    );
                }
                else
                {
                    query = query.Where(x => x.Object.ObjectType == EntityObjectType.Activity && x.Object.Activity.Hash == activityHash);
                }
            }

            if (request.Registration.HasValue)
            {
                Guid registration = request.Registration.Value;
                query = (
                    from statement in query
                    where statement.Context != null && statement.Context.Registration == registration
                    select statement
                );
            }

            if (request.Ascending.GetValueOrDefault())
            {
                query = query.OrderBy(x => x.Timestamp);
            }
            else
            {
                query = query.OrderByDescending(x => x.Timestamp);
            }

            int skipRows = 0;
            //if (!string.IsNullOrEmpty(request.MoreToken))
            //{
            //    if(_cache.TryGetValue(request.MoreToken, out int skipRows))
            //    {
            //        skipRows += request.Limit.Value;
            //    }
            //}

            if (request.Attachments.GetValueOrDefault())
            {
                query = query.Select(p => new StatementEntity
                {
                    FullStatement = p.FullStatement
                });
            }
            else
            {
                query = query.Select(p => new StatementEntity
                {
                    FullStatement = p.FullStatement,
                    Attachments = p.Attachments
                });
            }

            int pageSize = request.Limit ?? 1000;

            var pagedQuery = await query.Skip(skipRows).Take(pageSize)
                .GroupBy(p => new { TotalCount = query.Count() })
                .FirstOrDefaultAsync(cancellationToken);

            int totalCount = pagedQuery.Key.TotalCount;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            List<Statement> statements = pagedQuery.Select(p => _mapper.Map<Statement>(p)).ToList();

            var statementCollection = new StatementCollection(statements);

            string moreToken = totalPages > 1 ? Guid.NewGuid().ToString() : null;
            // TODO: Save query

            return new PagedStatementsResult(statementCollection, moreToken);
        }

        public async Task<Statement> Handle(GetStatementQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Statements
                    .Where(x => x.StatementId == request.StatementId && x.Voided == false);

            if (request.IncludeAttachments)
            {
                query = query.Include(x => x.Attachments)
                    .Select(x => new StatementEntity()
                    {
                        StatementId = x.StatementId,
                        FullStatement = x.FullStatement,
                        Attachments = x.Attachments
                    });
            }
            else
            {
                query = query.Select(x => new StatementEntity()
                {
                    StatementId = x.StatementId,
                    FullStatement = x.FullStatement
                });
            }

            StatementEntity statementEntity = await query.FirstOrDefaultAsync(cancellationToken);

            if (statementEntity == null)
            {
                return null;
            }

            return new Statement(statementEntity.FullStatement);
        }

        public async Task<DateTimeOffset?> Handle(GetConsistentThroughQuery request, CancellationToken cancellationToken)
        {
            var first = await _context.Statements.OrderByDescending(x => x.Stored)
                .FirstOrDefaultAsync(cancellationToken);
            return first?.Stored;
        }

        public Task<Statement> Handle(GetVoidedStatemetQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates statement without saving to database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Guid of the created statement</returns>
        public async Task<Guid> Handle(CreateStatementCommand request, CancellationToken cancellationToken)
        {
            if (!request.Statement.Id.HasValue)
            {
                request.Statement.Stamp();
            }

            // TODO: Move this logic elsewhere
            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var url = new Uri($"{httpRequest.Scheme}://{httpRequest.Host.Value}");
            if (request.Statement.Authority == null)
            {
                request.Statement.Authority = new Agent()
                {
                    Account = new xAPI.Account()
                    {
                        HomePage = url,
                        Name = "REPLACE ME"
                    }
                };
            }

            // Ensure statement version and stored date
            request.Statement.Version = request.Statement.Version ?? ApiVersion.GetLatest().ToString();
            request.Statement.Stored = request.Statement.Stored ?? DateTimeOffset.UtcNow;

            StatementEntity statement = _mapper.Map<StatementEntity>(request.Statement);
            statement.Verb = await _mediator.Send(MergeVerbCommand.Create(statement.Verb), cancellationToken);
            statement.Actor = await _mediator.Send(MergeActorCommand.Create(statement.Actor), cancellationToken);
            statement.FullStatement = request.Statement.ToJson();

            _context.Statements.Add(statement);

            return statement.StatementId.Value;
        }

        public async Task<ICollection<Guid>> Handle(CreateStatementsCommand request, CancellationToken cancellationToken)
        {
            var ids = new HashSet<Guid>();
            foreach (var statement in request.Statements)
            {
                ids.Add(await Handle(CreateStatementCommand.Create(statement), cancellationToken));
            }

            await _context.SaveChangesAsync(cancellationToken);

            return ids;
        }

        public async Task<Unit> Handle(PutStatementCommand request, CancellationToken cancellationToken)
        {
            Statement savedStatement = await Handle(GetStatementQuery.Create(request.StatementId), cancellationToken);

            request.Statement.Id = request.StatementId;

            await Handle(CreateStatementCommand.Create(request.Statement), cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return await Unit.Task;
        }

        public async Task<Unit> Handle(VoidStatementCommand request, CancellationToken cancellationToken)
        {
            IStatementBaseEntity voidingStatement = request.Statement;

            StatementRefEntity statementRef = voidingStatement.Object.StatementRef as StatementRefEntity;
            Guid? statementRefId = statementRef.Id;
            StatementEntity voidedStatement = _context.Statements
                .FirstOrDefault(x => x.StatementId == statementRefId);

            // Upon receiving a Statement that voids another, the LRS SHOULD NOT* reject the request on the grounds of the Object of that voiding Statement not being present.
            if (voidedStatement == null)
                return await Unit.Task; // Soft

            // Any Statement that voids another cannot itself be voided.
            if (voidedStatement.Verb.Id == xAPI.Verbs.Voided)
                return await Unit.Task; // Soft

            // voidedStatement has been voided, return.
            if (voidedStatement.Voided)
                return await Unit.Task; // Soft

            voidedStatement.Voided = true;

            _context.Statements.Update(voidedStatement);

            return await Unit.Task;
        }
    }
}
