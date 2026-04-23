namespace TravelTrek.Domain.Entities;

public class TripPlace
{
    public Guid Id { get; set; }
    public Guid TripDayId { get; set; }
    public string Xid { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Address { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public int OrderInDay { get; set; }

}