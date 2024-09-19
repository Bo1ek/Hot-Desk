namespace SoftwareMind.Application.Exceptions;

public class DeskAlreadyReservedException : Exception
{
    public DeskAlreadyReservedException(int deskId) : base($"Desk with id {deskId} not found")
    {
         
    }
}
