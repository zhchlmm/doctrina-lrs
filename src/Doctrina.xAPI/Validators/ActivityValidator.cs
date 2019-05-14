using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Definition).SetValidator(new ActivityDefinitionValidator());
        }
    }
}
