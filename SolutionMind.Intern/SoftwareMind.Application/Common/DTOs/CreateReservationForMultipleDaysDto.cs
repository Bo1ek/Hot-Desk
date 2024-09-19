namespace SoftwareMind.Application.Common.DTOs;

public class CreateReservationForMultipleDaysDto
{
    public int DeskId { get; set; }
    public string UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

