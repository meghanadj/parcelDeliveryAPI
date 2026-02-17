using Microsoft.EntityFrameworkCore;
using ParcelDelivery.Api.Models;

namespace ParcelDelivery.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Parcel> Parcels => Set<Parcel>();
    public DbSet<Recipient> Recipients => Set<Recipient>();
    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Mail", WeightLimit = 1.0 },
            new Department { Id = 2, Name = "Regular", WeightLimit = 10.0 },
            new Department { Id = 3, Name = "Heavy" }, // No limit implies > 10.0
            new Department { Id = 4, Name = "Insurance" } // Manual assignment only
        );

        // Parcels are now separate entities related to Order and Recipient
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Parcels)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Parcel>()
            .HasOne(p => p.Recipient)
            .WithMany(r => r.Parcels)
            .HasForeignKey(p => p.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Parcel>()
            .HasOne(p => p.Department)
            .WithMany()
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}