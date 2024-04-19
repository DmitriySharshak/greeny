namespace Greeny.Core.Validation
{
    public sealed class ValidationResult
    {
        public static ValidationResult SuccessResult = new ValidationResult();

        private readonly List<string> _fields = new List<string>();
        private readonly List<string> _messages = new List<string>();
        private readonly List<ValidatorResultCode> _resultCodes = new List<ValidatorResultCode>();

        protected ValidationResult()
        {

        }

        public ValidationResult(ValidatorResultCode code, string message, string fieldName)
        {
            this._isSucceedResult = false;
            _fields.Add(fieldName);
            _messages.Add(message);
            _resultCodes.Add(code);
        }

        private bool _isSucceedResult = true;

        


        public List<ValidatorResultCode> ResultCodes { get { return _resultCodes; } }

        public List<string> Messages { get { return _messages; } }

        public List<string> Fields { get { return _fields; } }

        public bool IsSucceed { get { return _isSucceedResult; } }

        public void AddError(ValidatorResultCode code)
        {
            _isSucceedResult = false;
            _resultCodes.Add(code);
        }

        public void Merge(ValidationResult result)
        {
            _fields.AddRange(result.Fields);
            _messages.AddRange(result.Messages);
            _resultCodes.AddRange(result.ResultCodes);
            _isSucceedResult &= result.IsSucceed;
        }
    }
}
