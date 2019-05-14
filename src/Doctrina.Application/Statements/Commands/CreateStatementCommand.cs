using AutoMapper;
using Doctrina.Application.Agents.Commands;
using Doctrina.Application.Verbs.Commands;
using Doctrina.Domain.Entities;
using Doctrina.Persistence;
using Doctrina.xAPI;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Statements.Commands
{
    public class CreateStatementCommand : IRequest<Guid>
    {
        public Statement Statement { get; set; }

        //public class Handler : IRequestHandler<CreateStatementCommand, Guid>
        //{
        //    private readonly DoctrinaDbContext _context;
        //    private readonly IMediator _mediator;
        //    private readonly IMapper _mapper;

        //    public Handler(DoctrinaDbContext context, IMediator mediator, IMapper mapper){
        //        _context = context;
        //        _mediator = mediator;
        //        _mapper = mapper;
        //    }

        //    public async Task<Guid> Handle(CreateStatementCommand request, CancellationToken cancellationToken)
        //    {
        //        StatementEntity statement = _mapper.Map<StatementEntity>(request.Statement);

        //        // Prevent conflic
        //        if (statement.StatementId != null)
        //        {
        //            // TODO: Statement Comparision Requirements
        //            /// https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#statement-comparision-requirements
        //            var exist = _context.Statements.Find(statement.StatementId);
        //            if (exist != null)
        //                return exist.StatementId;
        //        }
        //        else
        //        {
        //            statement.StatementId = Guid.NewGuid();
        //            statement.Timestamp = DateTimeOffset.UtcNow;
        //            statement.Stored = DateTimeOffset.UtcNow;
        //        }

        //        statement.Verb = await _mediator.Send(MergeVerbCommand.Create(statement.Verb));
        //        statement.Actor = await _mediator.Send(MergeActorCommand.Create(statement.Actor));

        //        _context.Statements.Add(statement);

        //        return statement.StatementId;
        //    }
        //}

        public static CreateStatementCommand Create(Statement statement)
        {
            return new CreateStatementCommand()
            {
                Statement = statement
            };
        }
    }
}
