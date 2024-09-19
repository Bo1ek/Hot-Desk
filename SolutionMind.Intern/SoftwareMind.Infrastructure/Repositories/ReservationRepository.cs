using SoftwareMind.Application.Exceptions;
using SoftwareMind.Infrastructure.Data;
using SoftwareMind.Infrastructure.DTOs;
using SoftwareMind.Infrastructure.Entities;
using SoftwareMind.Infrastructure.Helpers;

namespace SoftwareMind.Infrastructure.Repositories;
public interface IReservationRepository
{
    Task BookDeskForMultipleDays(CreateReservationForMultipleDaysDto createReservationDto, CancellationToken cancellationToken = default);
    bool IsReserved(int deskId, DateTime startDate, DateTime endDate);
    Task BookDeskForOneDay(int deskId, string userId, DateTime reservationDay, CancellationToken cancellationToken = default);
    bool Exists(int reservationId);
    bool IsReservedByUser(string userId, int reservationId);
    Task<Reservation> UpdateDesk(int deskId, string userId, int reservationId, CancellationToken cancellationToken = default);
    Task<List<Reservation>> GetListOfReservations();
}

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IDeskRepository _deskRepository;
    public ReservationRepository(ApplicationDbContext context, IDeskRepository deskRepository)
    {
        _deskRepository = deskRepository;
        _context = context;
    }
    public async Task BookDeskForMultipleDays(CreateReservationForMultipleDaysDto createReservationDto, CancellationToken cancellationToken = default)
    {
        if (_deskRepository.Exists(createReservationDto.DeskId) && !IsReserved(createReservationDto.DeskId, createReservationDto.StartDate, createReservationDto.EndDate))
        {
            var reservation = new Reservation
            {
                DeskId = createReservationDto.DeskId,
                UserId = createReservationDto.UserId,
                StartDate = DateTimeHelper.SetTimeToStartOfDay(createReservationDto.StartDate),
                EndDate = DateTimeHelper.SetTimeToEndOfDay(createReservationDto.EndDate),
            };
            await _context.Reservations.AddAsync(reservation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else if (!_deskRepository.Exists(createReservationDto.DeskId))
        {
            throw new DeskNotFoundException(createReservationDto.DeskId);
        }
        else if (IsReserved(createReservationDto.DeskId, createReservationDto.StartDate, createReservationDto.EndDate))
        {
            throw new DeskNotAvailableException(createReservationDto.DeskId);
        }
    }
    public async Task BookDeskForOneDay(int deskId, string userId, DateTime reservationDay, CancellationToken cancellationToken = default)
    {
        if (_deskRepository.Exists(deskId) && !IsReserved(deskId, reservationDay, reservationDay))
        {
            var reservation = new Reservation
            {
                DeskId = deskId,
                UserId = userId,
                StartDate = DateTimeHelper.SetTimeToStartOfDay(reservationDay),
                EndDate = DateTimeHelper.SetTimeToEndOfDay(reservationDay),
            };
            await _context.Reservations.AddAsync(reservation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else if (!_deskRepository.Exists(deskId))
        {
            throw new DeskNotFoundException(deskId);
        }
        else if (IsReserved(deskId, reservationDay, reservationDay))
        {
            throw new DeskNotAvailableException(deskId);
        }
    }
    public bool IsReserved(int deskId, DateTime startDate, DateTime endDate)
    {
        return _context.Reservations.Any(s =>
            s.DeskId == deskId &&
            ((s.StartDate <= startDate && s.EndDate >= startDate) ||
             (s.StartDate <= endDate && s.EndDate >= endDate) ||
             (s.StartDate >= startDate && s.EndDate <= endDate)));
    }
    public bool Exists(int reservationId)
    {
        return _context.Reservations.Any(r => r.Id == reservationId);
    }
    public bool IsReservedByUser(string userId, int reservationId)
    {
        return _context.Reservations.Any(r => r.UserId == userId && r.Id == reservationId);
    }
    public async Task<Reservation> GetReservationById(int reservationId)
    {
        return await _context.Reservations.FindAsync(reservationId);
    }

    public async Task<Reservation> UpdateDesk(int deskId, string userId, int reservationId, CancellationToken cancellationToken = default)
    {
        if (_deskRepository.Exists(deskId) && IsReservedByUser(userId, reservationId))
        {
            var reservation = await GetReservationById(reservationId);
            if (reservation.StartDate > DateTime.UtcNow.AddHours(24))
            {
                reservation.DeskId = deskId;
                await _context.SaveChangesAsync(cancellationToken);
                return reservation;
            }
            throw new ReservationIsTooSoonToUpdateDeskException(reservationId);
        }
        throw new DeskNotAvailableException(deskId);
    }

    public async Task<List<Reservation>> GetListOfReservations()
    {
        return _context.Reservations.ToList();
    }


}
