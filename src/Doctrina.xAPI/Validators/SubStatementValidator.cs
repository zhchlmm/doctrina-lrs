using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class SubStatementValidator : AbstractValidator<SubStatement>
    {
        public SubStatementValidator()
        {
            Include(new StatementBaseValidator());
            RuleFor(x => x.ObjectType).NotNull().NotEqual(ObjectType.SubStatement);
        }
    }
}
