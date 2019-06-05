using FluentValidation;
using FluentValidation.Results;

namespace Doctrina.xAPI.Validators
{
    public class StatementValidator : AbstractValidator<Statement>
    {
        public StatementValidator()
        {
            Include(new StatementBaseValidator());

            RuleFor(x => x.Object as SubStatement)
               .SetValidator(new SubStatementValidator())
               .When(x => x.Object != null && x.Object.ObjectType == ObjectType.SubStatement);

            // TODO: https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#requirements-14
            RuleFor(x => x.Authority).SetValidator(new AgentValidator())
                .When(x => x.Authority != null && x.Authority.ObjectType == ObjectType.Agent);

            RuleFor(x => x.Authority)
                .Must(auth =>
                {
                    var grp = auth as Group;
                    return grp.Member.Count == 2;
                })
                .When(x => x.Authority != null && x.Authority.ObjectType == ObjectType.Group)
                .WithMessage("When 3-legged OAuth, the group must have 2 Agents. The two Agents represent an application and user together.");

            //RuleFor(x => x).Custom((x, context) =>
            //{
            //    var failures = x.GetFailures();
            //    if (failures.Count > 0)
            //    {
            //        foreach(var failure in failures)
            //        {
            //            context.AddFailure(failure.Name, failure.Message);
            //        }
            //    }
            //});

            RuleFor(x => x.Object.ObjectType)
                .Equal(ObjectType.SubStatement)
                .When(x => x.Verb?.Id?.ToString() == Verbs.Voided)
                .WithMessage("When statement verb is voided, statement object must be StatementRef.");

            RuleFor(x => x.ParsingErrors).Custom((x, context) =>
            {
                foreach (var failure in x)
                {
                    context.AddFailure(failure.Name ?? context.DisplayName, failure.ErrorMessage);
                }
            });
        }
    }
}
