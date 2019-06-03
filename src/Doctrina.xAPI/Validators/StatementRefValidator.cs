using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class StatementRefValidator : AbstractValidator<StatementRef>
    {
        public StatementRefValidator()
        {
            RuleFor(x => x.ObjectType).Equal(ObjectType.StatementRef);
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.ParsingErrors).Custom((x, context) =>
            {
                foreach (var failure in x)
                {
                    context.AddFailure(failure.Name, failure.ErrorMessage);
                }
            });
        }
    }
}