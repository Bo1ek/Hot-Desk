namespace SoftwareMind.Infrastructure.Helpers;

public static class DateTimeHelper
{
    public static DateTime SetTimeToStartOfDay(this DateTime dateTime)
    {
        return new DateTime(
         dateTime.Year,
         dateTime.Month,
         dateTime.Day,
         0, 0, 0, 0);
    }
    public static DateTime SetTimeToEndOfDay(this DateTime dateTime)
    {
        return new DateTime(
         dateTime.Year,
         dateTime.Month,
         dateTime.Day,
         23, 59, 59, 999);
    }
}
