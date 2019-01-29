namespace Doctrina.xAPI.LRS.Validation
{
    public class ValidationError
    {
        public string Message { get; private set; }

        public ValidationError(string message)
        {
            Message = message;
        }
    }
}