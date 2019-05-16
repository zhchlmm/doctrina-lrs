using AutoMapper;
using Doctrina.Application.Agents.Commands;
using Doctrina.Application.Interfaces;
using Doctrina.Application.Statements.Commands;
using Doctrina.Application.Statements.Queries;
using Doctrina.Application.Verbs.Commands;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            StatementEntity entity = null;
            if (request.IncludeAttachments)
            {
                entity = await _context.Statements
                    .Include(x => x.Attachments)
                    .FirstOrDefaultAsync(x => x.StatementId == request.StatementId && x.Voided == false, cancellationToken);
            }
            else
            {
                entity = await _context.Statements
                    .FirstOrDefaultAsync(x => x.StatementId == request.StatementId && x.Voided == false, cancellationToken);
            }

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<Statement>(entity);
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

            // Prevent conflic
            if (request.Statement.Id.HasValue)
            {
                var savedStatement = await Handle(GetStatementQuery.Create(request.Statement.Id.Value), cancellationToken);
                if (savedStatement != null &&
                    !savedStatement.Equals(request.Statement))
                {
                    throw new ValidationException(new List<ValidationFailure>() {
                        new ValidationFailure(nameof(request.Statement.Id), "Conflict") { ErrorCode = "409"}
                    });
                    // TODO: Statement Comparision Requirements
                    /// https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#statement-comparision-requirements
                }
            }

            StatementEntity statement = _mapper.Map<StatementEntity>(request.Statement);
            statement.StatementId = Guid.NewGuid();
            statement.Timestamp = DateTimeOffset.UtcNow;
            statement.Stored = DateTimeOffset.UtcNow;
            statement.Verb = await _mediator.Send(MergeVerbCommand.Create(statement.Verb), cancellationToken);
            statement.Actor = await _mediator.Send(MergeActorCommand.Create(statement.Actor), cancellationToken);

            _context.Statements.Add(statement);

            return statement.StatementId;
        }

        public async Task<ICollection<Guid>> Handle(CreateStatementsCommand request, CancellationToken cancellationToken)
        {
            var ids = new HashSet<Guid>();
            foreach (var statement in request.Statements)
            {
                ids.Add(await Handle(CreateStatementCommand.Create(statement), cancellationToken));
            }
            return ids;
        }

        public async Task<Unit> Handle(PutStatementCommand request, CancellationToken cancellationToken)
        {
            Statement savedStatement = await Handle(GetStatementQuery.Create(request.StatementId), cancellationToken);

            request.Statement.Id = request.StatementId;

            if (savedStatement != null &&
                !savedStatement.Equals(request.Statement))
            {
                throw new ValidationException(new List<ValidationFailure>() {
                    new ValidationFailure(nameof(request.StatementId), "Conflict") { ErrorCode = "409"}
                });
            }
            else
            {
                await Handle(CreateStatementCommand.Create(request.Statement), cancellationToken);
            }

            return await Unit.Task;
        }

        public async Task<Unit> Handle(VoidStatementCommand request, CancellationToken cancellationToken)
        {
            var voidingStatement = request.Statement;

            var statementRefId = voidingStatement.ObjectStatementRefId.Value;
            var voidedStatement = _context.Statements
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
