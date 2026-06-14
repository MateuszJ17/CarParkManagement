using CarParkManagement.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Database.DbContext;

public class CarParkManagementDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public const string ConnectionStringName = "CarParkManagementDb";
    
    public CarParkManagementDbContext(DbContextOptions<CarParkManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<ParkingSpace> ParkingSpaces { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(x =>
        {
            x.HasKey(y => y.CarId);
            x.Property(y => y.CarId).ValueGeneratedOnAdd();
            x.Property(y => y.ParkingSpaceId).IsRequired();
            x.Property(y => y.RegistrationNumber).IsRequired().HasMaxLength(8);
            x.Property(y => y.Type).IsRequired();
            
            x.HasOne<ParkingSpace>().WithOne(y => y.Car).HasForeignKey<Car>(y => y.ParkingSpaceId);
        });
        
        modelBuilder.Entity<ParkingSpace>(x =>
        {
            x.HasKey(y => y.ParkingSpaceId);
            x.Property(y => y.ParkingSpaceId).ValueGeneratedOnAdd();
            x.Property(y => y.State).IsRequired().HasConversion<string>();
            x.Property(y => y.OccupiedSince).IsRequired(false);
        });
    }
}