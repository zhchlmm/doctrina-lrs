using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class StatementBaseValidator : AbstractValidator<IStatementBase>
    {
        public StatementBaseValidator()
        {
            RuleFor(x => x.Actor).NotEmpty();
            RuleFor(x => x.Verb).NotEmpty();
            Include(new StatementTargetValidator());
            RuleFor(x => x.Result).SetValidator(new ResultValidator());
            RuleFor(x => x.Context).SetValidator(new ContextValidator());
            RuleForEach(x => x.Attachments).SetValidator(new AttachmentValidator());
        }
    }
}
