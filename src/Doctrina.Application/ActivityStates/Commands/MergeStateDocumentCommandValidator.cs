using FluentValidation;

namespace Doctrina.Application.ActivityStates.Commands
{
    public class MergeStateDocumentCommandValidator : AbstractValidator<MergeStateDocumentCommand>
    {
        public MergeStateDocumentCommandValidator()
        {
        }
    }
}
