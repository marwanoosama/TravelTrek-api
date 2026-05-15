using TravelTrek.Domain.Common;

namespace TravelTrek.Application.Interfaces;

public interface ILLMService
{

    Task<Result<string>> GenerateAsync(string prompt, CancellationToken ct = default);
}
