using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftwareMind.Infrastructure.Entities;

namespace SoftwareMind.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Users = Set<User>();
    }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("Users");
        modelBuilder.Entity<IdentityRole>()
            .ToTable("Role");
        modelBuilder.Entity<IdentityUserRole<string>>()
            .ToTable("UserRoles")
            .HasKey(u => new { u.UserId, u.RoleId });
        
    }
}
