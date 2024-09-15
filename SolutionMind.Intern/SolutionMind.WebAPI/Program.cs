using Microsoft.EntityFrameworkCore;
using SoftwareMind.Infrastructure.Data;
using SoftwareMind.Infrastructure.Entities;
using SoftwareMind.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SoftwareMind"));
});
builder.Services.AddAuthorizationBuilder();
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
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
