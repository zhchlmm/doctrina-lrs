using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class StatementValidator : AbstractValidator<Statement>
    {
        public StatementValidator()
        {
            RuleFor(x => x.Actor).NotNull();
            RuleFor(x => x.Verb).NotNull();
            RuleFor(x => x.Object).NotNull();

            //// When issuing a Statement that voids another, the Object of that voiding Statement MUST have the "objectType" property set to StatementRef.
            //RuleFor(x => x.Object).Null()
            //    .WithMessage("When issuing a Statement that voids another, the Object of that voiding Statement MUST have the 'objectType' property set to StatementRef.");

            //// An LRS MUST consider a Statement it contains voided if and only if the Statement is not itself a voiding Statement and the LRS also contains a voiding Statement referring to the first Statement.
            //RuleFor(x => x.Verb.Id).NotEqual(xAPI.Verbs.Voided)
            //    .WithMessage("Any Statement that voids another must have the verb: \"" + xAPI.Verbs.Voided + "\"");
        }
    }
}
