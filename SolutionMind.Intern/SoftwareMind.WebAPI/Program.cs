using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SoftwareMind.Application.Common.Models;
using SoftwareMind.Infrastructure.Data;
using System.Reflection;
using Microsoft.AspNetCore.Identity.UI.Services;
using SoftwareMind.WebAPI.Services;
using SoftwareMind.WebAPI.Startup;
using SoftwareMind.WebAPI.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;


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
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRepositories();
builder.Services.FluentEmailInjection(builder.Configuration);
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None; // Allows cross-site cookies
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure HTTPS
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<User>();
app.Run();
