namespace Greeny.Core.Validation
{
    internal class CompositeValidator : IValidator
    {
        private readonly List<IValidator> validators = new List<IValidator>();

        public CompositeValidator Add(IValidator validator)
        {
            validators.Add(validator);
            return this;
        }

        public CompositeValidator AddRequired(string value, string fieldName)
        {
            validators.Add(new RequiredValidator(value, fieldName));
            return this;
        }

        public CompositeValidator Remove(IValidator validator)
        {
            validators.Remove(validator);
            return this;
        }

        public ValidationResult Validate()
        {
            ValidationResult result = ValidationResult.SuccessResult;
            foreach (IValidator validator in validators)
            {
                result.Merge(validator.Validate());
            }

            return result;
        }
    }
}
