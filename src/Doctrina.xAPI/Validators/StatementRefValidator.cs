using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class StatementRefValidator : AbstractValidator<StatementRef>
    {
        public StatementRefValidator()
        {
            RuleFor(x => x.ObjectType).Equal(ObjectType.StatementRef);
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}