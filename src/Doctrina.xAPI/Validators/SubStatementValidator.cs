using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class SubStatementValidator : AbstractValidator<SubStatement>
    {
        public SubStatementValidator()
        {
            Include(new StatementBaseValidator());

            RuleFor(x => x.Object).NotEmpty().DependentRules(() =>
            {
                RuleFor(x => x.Object.ObjectType).NotEqual(ObjectType.SubStatement)
                .WithMessage("A SubStatement MUST NOT contain a SubStatement of its own, i.e., cannot be nested.")
                .When(x => x.Object.ObjectType == ObjectType.SubStatement);
            });

            RuleFor(x => x.Failures).Custom((x, context) =>
            {
                foreach (var failure in x)
                {
                    context.AddFailure(failure.Name, failure.Message);
                }
            });
        }
    }
}
