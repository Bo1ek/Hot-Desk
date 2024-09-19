namespace SoftwareMind.Application.Common.Exceptions;
public class DeskNotFoundException : Exception
{
    public DeskNotFoundException(int deskId) : base($"Desk with id {deskId} not found")
    {
    }
}
