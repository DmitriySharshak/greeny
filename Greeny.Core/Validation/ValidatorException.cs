namespace Greeny.Core.Validation
{
    public class ValidatorException : Exception
    {
        private Dictionary<string, string> errors = new Dictionary<string, string>();

        public ValidatorException(ValidationResult validationResult)
        {
            var count = validationResult.Fields.Count;

            for (var i = 0; i < count; i++)
            {
                var fieldName = validationResult.Fields[i];
                var message = validationResult.Messages[i];
                errors.Add(fieldName, message);
            }
        }

        public Dictionary<string, string> Errors { get { return errors; } }
    }
}
