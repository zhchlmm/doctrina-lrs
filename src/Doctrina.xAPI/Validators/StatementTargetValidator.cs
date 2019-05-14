using FluentValidation;
using FluentValidation.Results;

namespace Doctrina.xAPI.Validators
{
    public class StatementTargetValidator : AbstractValidator<IStatementBase>
    {
        public StatementTargetValidator()
        {
            RuleFor(x => x.Target).NotNull().Custom((target, context) =>
            {
                ValidationResult result = null;
                
                switch (target.ObjectType)
                {
                    case ObjectType.Agent:
                        var agentValidator = new AgentValidator();
                        result = agentValidator.Validate((Agent)context.InstanceToValidate);
                        break;
                    case ObjectType.Group:
                        var groupValidator = new GroupValidator();
                        result = groupValidator.Validate((Group)context.InstanceToValidate);
                        break;
                    case ObjectType.Activity:
                        var ActivityValidator = new ActivityValidator();
                        result = ActivityValidator.Validate((Activity)context.InstanceToValidate);
                        break;
                    case ObjectType.SubStatement:
                        var SubStatementValidator = new SubStatementValidator();
                        result = SubStatementValidator.Validate((SubStatement)context.InstanceToValidate);
                        break;
                    case ObjectType.StatementRef:
                        break;
                    default:
                        break;
                }

                foreach(var failure in result.Errors)
                {
                    context.AddFailure(failure);
                }

            });
        }
    }
}
