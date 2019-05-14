using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class SubStatementValidator : StatementBaseValidator<SubStatement>
    {
        public SubStatementValidator()
        {
            RuleFor(x => x.ObjectType).NotNull().NotEqual(ObjectType.SubStatement);
        }
    }
}
