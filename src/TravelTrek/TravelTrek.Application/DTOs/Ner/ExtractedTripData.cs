namespace TravelTrek.Application.DTOs.Ner;

public class ExtractedTripData
{
    public List<string> Locations { get; set; } = new();
    public List<string> Dates { get; set; } = new();
    public List<string> Durations { get; set; } = new();
    public List<string> Budgets { get; set; } = new();
    public List<string> GroupSizes { get; set; } = new();
    public List<string> TravelTypes { get; set; } = new();
    public List<string> Activities { get; set; } = new();
}
