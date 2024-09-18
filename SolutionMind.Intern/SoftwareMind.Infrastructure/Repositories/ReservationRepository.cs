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
        else throw new Exception("Desk is reserved or does not exist");
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
        else throw new Exception("Desk is reserved or does not exist");
    }
    public bool IsReserved(int deskId, DateTime startDate, DateTime endDate)
    {
        return _context.Reservations.Any(s =>
            s.DeskId == deskId &&
            ((s.StartDate <= startDate && s.EndDate >= startDate) ||
             (s.StartDate <= endDate && s.EndDate >= endDate) ||
             (s.StartDate >= startDate && s.EndDate <= endDate)));
    }

}
