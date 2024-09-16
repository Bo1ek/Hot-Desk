using SoftwareMind.Infrastructure.Data;
using SoftwareMind.Infrastructure.DTOs;
using SoftwareMind.Infrastructure.Entities;

namespace SoftwareMind.Infrastructure.Repositories;
public interface ILocationRepository
{
    Task CreateAsync(CreateLocationDto createLocationDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(LocationDto locationDto, CancellationToken cancellationToken = default);
    Task RemoveAsync(int locationId, CancellationToken cancellationToken = default);
}

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;
    public LocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(CreateLocationDto createLocationDto, CancellationToken cancellationToken = default)
    {
        var locationEntity = new Location
        {
            City = createLocationDto.City
        };
        await _context.Locations.AddAsync(locationEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(LocationDto locationDto, CancellationToken cancellationToken = default)
    {
        var location = await _context.Locations.FindAsync(locationDto.Id, cancellationToken);
        if (location == null)
        {
            throw new Exception("Location not found");
        }
        location.City = locationDto.City;
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task RemoveAsync(int locationId, CancellationToken cancellationToken = default)
    {
        var locationToRemove = await _context.Locations.FindAsync(locationId, cancellationToken);
        if (locationToRemove != null) 
        {
            _context.Remove(locationToRemove);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

