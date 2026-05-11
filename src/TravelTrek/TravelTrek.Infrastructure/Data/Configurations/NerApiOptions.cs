using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Infrastructure.Data.Configurations;

public class NerApiOptions
{
    public const string SectionName = "NerApi";

    [Required(ErrorMessage = "NerApi BaseUrl is required.")]
    public string BaseUrl { get; set; } = default!;
}
