using SoftwareMind.Infrastructure.Repositories;

namespace SoftwareMind.WebAPI.Startup;

public static class DependencyInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IDeskRepository, DeskRepository>();
    }
}