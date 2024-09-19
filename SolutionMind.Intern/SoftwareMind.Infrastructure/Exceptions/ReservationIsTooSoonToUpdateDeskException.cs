namespace SoftwareMind.Infrastructure.Exceptions;
public class ReservationIsTooSoonToUpdateDeskException : Exception
{
    public ReservationIsTooSoonToUpdateDeskException(int reservationId) : base($"You cannot update a reservation" +
        $" with a reservation id: {reservationId} because it starts less than 24 hours away")
    {
        
    }
}
