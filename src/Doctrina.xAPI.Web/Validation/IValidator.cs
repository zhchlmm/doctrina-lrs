using System.Collections.Generic;

namespace Doctrina.xAPI.LRS.Validation
{
    public interface IValidator<T>
    {
        IList<ValidationError> Errors { get; set; }
    }
}
