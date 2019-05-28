using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class SubStatementValidator : AbstractValidator<SubStatement>
    {
        public SubStatementValidator()
        {
            Include(new StatementBaseValidator());
            RuleFor(x => x.ObjectType).NotNull().NotEqual(ObjectType.SubStatement)
                .WithMessage("A SubStatement MUST NOT contain a SubStatement of its own, i.e., cannot be nested.");
        }
    }
}
