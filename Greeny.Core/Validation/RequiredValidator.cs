using Greeny.Common.Enums;

namespace Greeny.Core.Validation
{
    internal class RequiredValidator : IValidator
    {
        private readonly string _fieldName;
        private readonly string _value;
        private readonly string _message;

        public RequiredValidator(string value, string fieldName)
        {
            _value = value;
            _fieldName = fieldName;
            _message = string.Format(ValidatorResultCode.Required.GetDescription(), fieldName);
        }

        public ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return new ValidationResult(ValidatorResultCode.Required, _message, _fieldName);

            return ValidationResult.SuccessResult;
        }
    }
}
