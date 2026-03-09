namespace TravelTrek.Domain.Common
{
    /// <summary>
    /// Represents the result of an operation without a return value
    /// </summary>
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException("Successful result cannot have an error");
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException("Failed result must have an error");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Indicates if the operation failed
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// The error that occurred (Error.None if successful)
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Creates a successful result
        /// </summary>
        public static Result Success() => new(true, Error.None);

        /// <summary>
        /// Creates a failed result with an error
        /// </summary>
        public static Result Failure(Error error) => new(false, error);

        /// <summary>
        /// Creates a successful result with a value
        /// </summary>
        public static Result<TValue> Success<TValue>(TValue value) =>
            new(value, true, Error.None);

        /// <summary>
        /// Creates a failed result with an error
        /// </summary>
        public static Result<TValue> Failure<TValue>(Error error) =>
            new(default, false, error);

        /// <summary>
        /// Creates a validation failure result with multiple errors
        /// </summary>
        public static Result<TValue> ValidationFailure<TValue>(Error[] errors) =>
            new(default, false, Error.Validation("Validation.Failed", "One or more validation errors occurred"), errors);
    }

    /// <summary>
    /// Represents the result of an operation with a return value
    /// </summary>
    /// <typeparam name="TValue">The type of the returned value</typeparam>
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error, Error[]? validationErrors = null)
            : base(isSuccess, error)
        {
            _value = value;
            ValidationErrors = validationErrors ?? Array.Empty<Error>();
        }

        /// <summary>
        /// The value returned by the operation (only available if successful)
        /// </summary>
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access value of a failed result");

        /// <summary>
        /// Collection of validation errors (empty if not a validation failure)
        /// </summary>
        public Error[] ValidationErrors { get; }

        /// <summary>
        /// Indicates if this is a validation failure with multiple errors
        /// </summary>
        public bool HasValidationErrors => ValidationErrors.Length > 0;

        public static implicit operator Result<TValue>(TValue value) => Success(value);

        /// <summary>
        /// Executes an action based on the result state
        /// </summary>
        public TResult Match<TResult>(
            Func<TValue, TResult> onSuccess,
            Func<Error, TResult> onFailure)
        {
            return IsSuccess ? onSuccess(_value!) : onFailure(Error);
        }

        /// <summary>
        /// Executes an async action based on the result state
        /// </summary>
        public async Task<TResult> MatchAsync<TResult>(
            Func<TValue, Task<TResult>> onSuccess,
            Func<Error, Task<TResult>> onFailure)
        {
            return IsSuccess ? await onSuccess(_value!) : await onFailure(Error);
        }

        /// <summary>
        /// Maps the value to a new type if successful
        /// </summary>
        public Result<TOutput> Map<TOutput>(Func<TValue, TOutput> mapper)
        {
            return IsSuccess ? Success(mapper(_value!)) : Failure<TOutput>(Error);
        }

        /// <summary>
        /// Binds the result to another operation
        /// </summary>
        public Result<TOutput> Bind<TOutput>(Func<TValue, Result<TOutput>> binder)
        {
            return IsSuccess ? binder(_value!) : Failure<TOutput>(Error);
        }

        /// <summary>
        /// Async version of Bind
        /// </summary>
        public async Task<Result<TOutput>> BindAsync<TOutput>(Func<TValue, Task<Result<TOutput>>> binder)
        {
            return IsSuccess ? await binder(_value!) : Failure<TOutput>(Error);
        }
    }
}