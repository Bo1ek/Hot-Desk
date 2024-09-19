namespace SoftwareMind.Application.Exceptions;

public class LocationNotFoundException : Exception
{
    public LocationNotFoundException(int locationId) : base($"Location with id {locationId} not found.")
    {
        
    }
}
