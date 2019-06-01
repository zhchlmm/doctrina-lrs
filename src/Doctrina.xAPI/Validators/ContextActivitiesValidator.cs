using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Validators
{
    public class ContextActivitiesValidator : AbstractValidator<ContextActivities>
    {
        public ContextActivitiesValidator()
        {
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
