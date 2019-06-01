using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class ResultValidator : AbstractValidator<Result>
    {
        public ResultValidator()
        {
            RuleFor(x => x.Score).SetValidator(new ScoreValidator()).When(x => x.Score != null);

            RuleFor(x => x.Extensions).SetValidator(new ExtensionsValidator()).When(x => x.Extensions != null);

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
