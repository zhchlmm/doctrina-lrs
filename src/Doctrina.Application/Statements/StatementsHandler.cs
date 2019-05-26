using AutoMapper;
using Doctrina.Application.Agents.Commands;
using Doctrina.Application.Interfaces;
using Doctrina.Application.Statements.Commands;
using Doctrina.Application.Statements.Queries;
using Doctrina.Application.Verbs.Commands;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using Doctrina.xAPI.Json;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Statements
{
    public class StatementsHandler :
        IRequestHandler<GetStatementsQuery, ICollection<Statement>>,
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

        public StatementsHandler(IDoctrinaDbContext context, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        public Task<ICollection<Statement>> Handle(GetStatementsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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

            var statement = JsonConvert.DeserializeObject<Statement>(statementEntity.FullStatement);
            foreach (var attachmentEntity in statementEntity.Attachments)
            {
                if(statement.Attachments.TryGetAttachment(attachmentEntity.SHA2, out Attachment attachment))
                {
                    attachment.SetPayload(attachmentEntity.Payload);
                }
            }

            return statement;
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

            StatementEntity statement = _mapper.Map<StatementEntity>(request.Statement);
            //statement.StatementId = statement.StatementId ?? Guid.NewGuid();
            //statement.Timestamp = DateTimeOffset.UtcNow;
            //statement.Stored = DateTimeOffset.UtcNow;
            statement.Verb = await _mediator.Send(MergeVerbCommand.Create(statement.Verb), cancellationToken);
            statement.Actor = await _mediator.Send(MergeActorCommand.Create(statement.Actor), cancellationToken);
            statement.Version = !string.IsNullOrEmpty(statement.Version) ? statement.Version : ApiVersion.GetLatest().ToString();
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
