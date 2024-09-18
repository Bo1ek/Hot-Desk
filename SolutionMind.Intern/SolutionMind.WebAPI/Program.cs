using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SoftwareMind.Infrastructure.Data;
using SoftwareMind.Infrastructure.Entities;
using SoftwareMind.Infrastructure.Repositories;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "SoftwareMind.Intern",
            Description = "An intuitive system to automate the reservation of desks in offices through an easy-to-use online booking system.",
            Version = "v1",
        });
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
    o.IncludeXmlComments(filePath);
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SoftwareMind"));
});
builder.Services.AddAuthorizationBuilder();
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IDeskRepository, DeskRepository>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<User>();
app.Run();
