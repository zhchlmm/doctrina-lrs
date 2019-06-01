using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(x => x.HomePage).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();

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
