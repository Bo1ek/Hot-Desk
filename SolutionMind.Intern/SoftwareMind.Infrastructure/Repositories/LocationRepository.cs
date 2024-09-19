using SoftwareMind.Application.Exceptions;
using SoftwareMind.Infrastructure.Data;
using SoftwareMind.Infrastructure.DTOs;
using SoftwareMind.Infrastructure.Entities;

namespace SoftwareMind.Infrastructure.Repositories;
public interface ILocationRepository
{
    Task CreateAsync(CreateLocationDto createLocationDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(LocationDto locationDto, CancellationToken cancellationToken = default);
    Task RemoveAsync(int locationId, CancellationToken cancellationToken = default);
    bool Exists(int locationId);
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
        if (!Exists(locationDto.Id))
        {
            throw new LocationNotFoundException(locationDto.Id);
        }
        location.City = locationDto.City;
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task RemoveAsync(int locationId, CancellationToken cancellationToken = default)
    {
        var locationToRemove = await _context.Locations.FindAsync(locationId, cancellationToken);
        if (locationToRemove == null) 
        {
            throw new LocationNotFoundException(locationId);
        }
        _context.Remove(locationToRemove);
        await _context.SaveChangesAsync(cancellationToken);

    }
    public bool Exists(int locationId)
    {
        return  _context.Locations.Any(l => l.Id == locationId);
    }
}

