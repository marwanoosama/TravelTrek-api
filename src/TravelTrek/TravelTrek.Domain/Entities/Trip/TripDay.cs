namespace TravelTrek.Domain.Entities;

public class TripDay
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public int DayNumber { get; set; }

    public List<TripPlace> Places { get; set; }
}