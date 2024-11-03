using DriversManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DriversManagement.API.Data
{
    public class DriversContext : DbContext
    {
        public DriversContext(DbContextOptions<DriversContext> options) : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleCategory> VehicleCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.FirstName).IsRequired().HasMaxLength(20);
                entity.Property(d => d.LastName).IsRequired();
                entity.HasOne(d => d.Category)
                    .WithMany(c => c.Drivers);
                
                entity.HasData(
                    new Driver { Id = 1, FirstName = "John", LastName = "Doe", CategoryId = 1},
                    new Driver { Id = 2, FirstName = "Jane", LastName = "Smith", CategoryId = 2}
                );
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Model).IsRequired();
                entity.Property(v => v.Engine).IsRequired();
                entity.HasOne(v => v.Driver)
                    .WithMany(d => d.Vehicles);
                
                entity.HasData(
                    new Vehicle { Id = 1, Model = "Toyota Camry", Engine = "V6", Year = 2006, DriverId = 1 },
                    new Vehicle { Id = 2, Model = "Honda Accord", Engine = "I4", Year = 2008, DriverId = 2 }
                );
            });

            modelBuilder.Entity<VehicleCategory>(entity =>
            {
                entity.HasKey(vc => vc.Id);
                entity.Property(vc => vc.Symbol).IsRequired();
                entity.HasMany(vc => vc.Drivers)
                    .WithOne(d => d.Category);
                
                entity.HasData(
                    new VehicleCategory { Id = 1, Symbol = "Sedan" },
                    new VehicleCategory { Id = 2, Symbol = "SUV" }
                );
            });
        }
    }
}