namespace SoftwareMind.Infrastructure.Entities;

public class Location
{
    public int Id { get; set; }
    public required string City { get; set; }
    public List<Desk> Desks { get; set; } = [];
}
