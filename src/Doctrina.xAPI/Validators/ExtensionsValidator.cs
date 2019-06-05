using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class ExtensionsValidator : AbstractValidator<ExtensionsDictionary>
    {
        public ExtensionsValidator()
        {
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
