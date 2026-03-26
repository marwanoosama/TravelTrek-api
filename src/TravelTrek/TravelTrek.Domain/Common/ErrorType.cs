namespace TravelTrek.Domain.Common
{
    /// <summary>
    /// Defines the type of error that occurred
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// Error caused by invalid request/input
        /// </summary>
        Validation,

        /// <summary>
        /// Error when resource is not found
        /// </summary>
        NotFound,

        /// <summary>
        /// Error when operation conflicts with current state
        /// </summary>
        Conflict,

        /// <summary>
        /// Error when user lacks permission
        /// </summary>
        Unauthorized,

        /// <summary>
        /// Error when user is authenticated but forbidden
        /// </summary>
        Forbidden,

        /// <summary>
        /// Unexpected internal server error
        /// </summary>
        Internal,

        /// <summary>
        /// External service error (API, Database, etc.)
        /// </summary>
        External
    }
}