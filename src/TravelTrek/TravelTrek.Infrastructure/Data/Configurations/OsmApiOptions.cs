using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Infrastructure.Data.Configurations;

public class OsmApiOptions
{
    public const string SectionName = "Osm";

    [Required(ErrorMessage = "OSM NominatimBaseUrl is required.")]
    public string NominatimBaseUrl { get; set; } = default!;

    [Required(ErrorMessage = "OSM OverpassBaseUrl is required.")]
    public string OverpassBaseUrl { get; set; } = default!;
}
