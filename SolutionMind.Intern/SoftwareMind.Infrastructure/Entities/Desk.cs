namespace SoftwareMind.Infrastructure.Entities;

public class Desk
{
    public int Id { get; set; }
    public bool IsAvailable { get; set; }
    public Location? Location { get; set; }
    public User? User { get; set; }
}
