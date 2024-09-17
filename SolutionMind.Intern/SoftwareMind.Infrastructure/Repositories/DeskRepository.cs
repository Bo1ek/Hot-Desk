using SoftwareMind.Infrastructure.Data;
using SoftwareMind.Infrastructure.DTOs;
using SoftwareMind.Infrastructure.Entities;
using SoftwareMind.Infrastructure.Exceptions;

namespace SoftwareMind.Infrastructure.Repositories;

public interface IDeskRepository
{
    Task CreateAsync(CreateDeskDto createDeskDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(LocationDto locationDto, CancellationToken cancellationToken = default);
    Task RemoveAsync(int locationId, CancellationToken cancellationToken = default);
    bool CheckIfReserved(int deskId);
    bool CheckIfExists(int deskId);
}
public class DeskRepository : IDeskRepository
{
    private readonly ApplicationDbContext _context;
    public DeskRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(CreateDeskDto createDeskDto, CancellationToken cancellationToken = default)
    {
        var deskEntity = new Desk
        {
            LocationId = createDeskDto.LocationId,
            IsAvailable = true
        };
        await _context.AddAsync(deskEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(int deskId, CancellationToken cancellationToken = default)
    {
        var deskToRemove = await _context.Desks.FindAsync(deskId);
        if (deskToRemove == null)
        {
            throw new DeskNotFoundException(deskId);
        }
        _context.Remove(deskToRemove);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(LocationDto locationDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public bool CheckIfReserved(int deskId)
    {
        return _context.Desks.Any(d => d.Id == deskId && d.IsAvailable == false);
    }
    public bool CheckIfExists(int deskId)
    {
        return _context.Desks.Any(d => d.Id == deskId);
    }

}
