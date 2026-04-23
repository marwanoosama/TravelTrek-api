namespace TravelTrek.Domain.Entities;

public class Trip
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Destination { get; set; }
    public string Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string TripType { get; set; }
    public int DurationDays { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<TripDay> Days { get; set; }
}