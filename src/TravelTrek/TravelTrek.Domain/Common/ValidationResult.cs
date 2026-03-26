using TravelTrek.Domain.Abstractions;

namespace TravelTrek.Domain.Common
{
    /// <summary>
    /// Represents a validation result with multiple errors
    /// </summary>
    public sealed class ValidationResult : Result, IValidationResult
    {
        private ValidationResult(Error[] errors)
            : base(false, Error.Validation("Validation.Failed", "One or more validation errors occurred"))
        {
            Errors = errors;
        }

        public Error[] Errors { get; }

        /// <summary>
        /// Creates a validation result from an array of errors
        /// </summary>
        public static ValidationResult WithErrors(Error[] errors) => new(errors);

        /// <summary>
        /// Creates a validation result from a single error
        /// </summary>
        public static ValidationResult WithError(Error error) => new(new[] { error });
    }

    /// <summary>
    /// Generic validation result with multiple errors
    /// </summary>
    public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
    {
        private ValidationResult(Error[] errors)
            : base(default, false, Error.Validation("Validation.Failed", "One or more validation errors occurred"), errors)
        {
            Errors = errors;
        }

        public Error[] Errors { get; }

        /// <summary>
        /// Creates a validation result from an array of errors
        /// </summary>
        public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
    }
}