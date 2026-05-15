using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Infrastructure.Data.Configurations;

public class OpenAIApiOptions
{
    public const string SectionName = "OpenAI";

    [Required(ErrorMessage = "OpenAI BaseUrl is required.")]
    public string BaseUrl { get; set; } = default!;

    [Required(ErrorMessage = "OpenAI ApiKey is required.")]
    public string ApiKey { get; set; } = default!;

    [Required(ErrorMessage = "OpenAI Model is required.")]
    public string Model { get; set; } = default!;
}
