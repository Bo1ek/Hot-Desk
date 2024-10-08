﻿using SoftwareMind.Application.Common.DTOs;
using SoftwareMind.Application.Common.Exceptions;
using SoftwareMind.Application.Common.Models;
using SoftwareMind.Infrastructure.Data;

namespace SoftwareMind.Infrastructure.Repositories;

public interface IDeskRepository
{
    Task CreateAsync(CreateDeskDto createDeskDto, CancellationToken cancellationToken = default);
    Task RemoveAsync(int locationId, CancellationToken cancellationToken = default);
    bool IsAvailable(int deskId);
    bool Exists(int deskId);
    Task<Desk> MakeUnavailable(int deskId, CancellationToken cancellationToken = default);
    Task<List<Desk>> getAllAvailableDesks();
    Task<List<Desk>> getAllUnavailableDesks();
    Task<List<Desk>> getDesksByLocation(int locationId);

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

    public bool IsAvailable(int deskId)
    {
        return _context.Desks.Any(d => d.IsAvailable == true && d.Id == deskId);
    }

    public bool Exists(int deskId)
    {
        return _context.Desks.Any(d => d.Id == deskId);
    }

    public async Task<Desk> MakeUnavailable(int deskId, CancellationToken cancellationToken = default)
    {
        var desk = await _context.Desks.FindAsync(deskId, cancellationToken);
        if (!Exists(deskId))
        {
            throw new DeskNotFoundException(deskId);
        }
        else if (Exists(deskId) && !IsAvailable(deskId))
        {
            throw new DeskAlreadyReservedException(deskId);
        }
        desk.IsAvailable = false;
        await _context.SaveChangesAsync(cancellationToken);
        return desk;
    }

    public async Task<List<Desk>> getAllAvailableDesks()
    {
        return _context.Desks.Where(d => d.IsAvailable == true).ToList();
    }

    public async Task<List<Desk>> getAllUnavailableDesks()
    {
        return _context.Desks.Where(d => d.IsAvailable == false).ToList();
    }

    public async Task<List<Desk>> getDesksByLocation(int locationId)
    {
        return _context.Desks.Where(d => d.Location.Id == locationId ).ToList();
    }
}
