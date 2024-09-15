using SoftwareMind.Infrastructure.Data;
using SoftwareMind.Infrastructure.DTOs;
using SoftwareMind.Infrastructure.Entities;

namespace SoftwareMind.Infrastructure.Repositories;
public interface ILocationRepository
{
    Task CreateAsync(LocationDto locationDto);
    Task UpdateAsync(LocationDto locationDto);
    Task RemoveAsync(int locationId);
}

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;
    public LocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(LocationDto locationDto)
    {
        var locationEntity = new Location
        {
            Id = locationDto.Id,
            City = locationDto.City
        };
        await _context.Locations.AddAsync(locationEntity);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(LocationDto locationDto)
    {
        _context.Update(locationDto);
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

