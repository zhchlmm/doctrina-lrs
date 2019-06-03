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
