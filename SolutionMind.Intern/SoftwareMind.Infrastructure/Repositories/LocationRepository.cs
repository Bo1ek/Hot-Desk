using SoftwareMind.Infrastructure.Data;
using SoftwareMind.Infrastructure.Entities;

namespace SoftwareMind.Infrastructure.Repositories;
public interface ILocationRepository
{
    Task CreateAsync(Location location);
    Task UpdateAsync(Location location);
    Task RemoveAsync(int locationId);
}

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;
    public LocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(Location location)
    {
        var locationEntity = new Location
        {
            Id = location.Id,
            City = location.City,
            Desks = location.Desks
        };
        await _context.Locations.AddAsync(locationEntity);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Location location)
    {
        _context.Update(location);
        await _context.SaveChangesAsync();
    }
    public async Task RemoveAsync(int locationId)
    {
        var locationToRemove = await _context.Locations.FindAsync(locationId);
        if (locationToRemove != null) 
        {
            _context.Remove(locationToRemove);
            await _context.SaveChangesAsync();
        }
    }
}

