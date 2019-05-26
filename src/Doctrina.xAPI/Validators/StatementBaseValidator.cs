using FluentValidation;
using FluentValidation.Results;

namespace Doctrina.xAPI.Validators
{
    public class StatementBaseValidator : AbstractValidator<IStatementBase>
    {
        public StatementBaseValidator()
        {
            RuleFor(x => x.Actor).NotEmpty();
            RuleFor(x => x.Verb).NotEmpty();
            RuleFor(x => x.Object).NotNull().Custom((target, context) =>
            {
                ValidationResult result = null;

                if (target.ObjectType == ObjectType.Agent)
                {
                    var agentValidator = new AgentValidator();
                    result = agentValidator.Validate((Agent)target);
                }
                else if (target.ObjectType == ObjectType.Group)
                {
                    var groupValidator = new GroupValidator();
                    result = groupValidator.Validate((Group)target);
                }
                else if (target.ObjectType == ObjectType.Activity)
                {
                    var ActivityValidator = new ActivityValidator();
                    result = ActivityValidator.Validate((Activity)target);
                }
                else if (target.ObjectType == ObjectType.SubStatement)
                {
                    var SubStatementValidator = new SubStatementValidator();
                    result = SubStatementValidator.Validate((SubStatement)target);
                }
                else if (target.ObjectType == ObjectType.StatementRef)
                {
                    var statementRefValidator = new StatementRefValidator();
                    result = statementRefValidator.Validate((StatementRef)target);
                }

                foreach (var failure in result.Errors)
                {
                    context.AddFailure(failure);
                }
            });
            RuleFor(x => x.Result).SetValidator(new ResultValidator());
            RuleFor(x => x.Context).SetValidator(new ContextValidator());
            RuleForEach(x => x.Attachments).SetValidator(new AttachmentValidator());

            RuleFor(x => x).Must(statement =>
            {
                return string.IsNullOrWhiteSpace(statement.Context?.Revision) ||
                    statement.Object?.ObjectType == ObjectType.Activity;
            }).WithMessage("A Statement cannot contain both a 'revision' property in its 'context' property and have the value of the 'object' property's 'objectType' be anything but 'Activity'");

            RuleFor(x => x).Must(statement =>
            {
                return string.IsNullOrWhiteSpace(statement.Context?.Platform) ||
                    statement.Object?.ObjectType == ObjectType.Activity;
            }).WithMessage("A Statement cannot contain both a 'platform' property in its 'context' property and have the value of the 'object' property's 'objectType' be anything but 'Activity'");
        }
    }
}
