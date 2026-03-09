using TravelTrek.Domain.Common;

namespace TravelTrek.Domain.Abstractions
{
    /// <summary>
    /// Marker interface for validation results
    /// </summary>
    public interface IValidationResult
    {
        Error[] Errors { get; }
    }
}