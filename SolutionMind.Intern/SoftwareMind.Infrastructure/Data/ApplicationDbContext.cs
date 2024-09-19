using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftwareMind.Infrastructure.Entities;

namespace SoftwareMind.Infrastructure.Data;

public sealed class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public new DbSet<User>? Users { get; set; }
    public DbSet<Desk> Desks { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles").HasKey(u => new { u.UserId, u.RoleId });

        modelBuilder.Entity<Location>()
            .ToTable("Locations")
            .HasMany<Desk>()
            .WithOne(d => d.Location)
            .HasForeignKey(d => d.LocationId)
            .IsRequired();

        modelBuilder.Entity<Desk>()
            .ToTable("Desks");

        modelBuilder.Entity<Reservation>()
            .ToTable("Reservations")
            .HasOne(r => r.Desk)
            .WithMany()
            .HasForeignKey(r => r.DeskId)
            .IsRequired();

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .IsRequired();

    }
}
