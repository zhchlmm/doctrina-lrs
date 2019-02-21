using FluentValidation;

namespace Doctrina.Application.Verbs.Commands
{
    public class MergeVerbCommandValidator : AbstractValidator<MergeVerbCommand>
    {
        public MergeVerbCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            //TODO: RuleForEach(x => x.Display);
        }
    }
}
