namespace SoftwareMind.Application.Exceptions;

public class DeskNotAvailableException : Exception
{
    public DeskNotAvailableException(int deskId) : base($"Desk with id {deskId} is not available." +
        $"Please check if Desk exists and is available to make a reservation for entered time.")
    {
        
    }
}
