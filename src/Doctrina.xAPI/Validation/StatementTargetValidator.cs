using FluentValidation;
using FluentValidation.Results;

namespace Doctrina.xAPI.Validation
{
    public class StatementTargetValidator : AbstractValidator<IStatementBase>
    {
        public StatementTargetValidator()
        {
            RuleFor(x => x.Object).NotNull().Custom((target, context) =>
            {
                ValidationResult result = null;

                if (target.ObjectType == ObjectType.Agent)
                {
                    var agentValidator = new AgentValidator();
                    result = agentValidator.Validate((Agent)context.InstanceToValidate);
                }
                else if (target.ObjectType == ObjectType.Group)
                {
                    var groupValidator = new GroupValidator();
                    result = groupValidator.Validate((Group)context.InstanceToValidate);
                }
                else if (target.ObjectType == ObjectType.Activity)
                {
                    var ActivityValidator = new ActivityValidator();
                    result = ActivityValidator.Validate((Activity)context.InstanceToValidate);
                }
                else if (target.ObjectType == ObjectType.SubStatement)
                {
                    var SubStatementValidator = new SubStatementValidator();
                    result = SubStatementValidator.Validate((SubStatement)context.InstanceToValidate);
                }
                else if (target.ObjectType == ObjectType.StatementRef)
                {

                }

                foreach (var failure in result.Errors)
                {
                    context.AddFailure(failure);
                }
            });

            //RuleFor(x => x.Failures).Custom((x, context) =>
            //{
            //    foreach (var failure in x)
            //    {
            //        context.AddFailure(failure.Name, failure.Message);
            //    }
            //});
        }
    }
}
