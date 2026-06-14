using CarParkManagement.Database.DbContext;
using CarParkManagement.Database.Entities;
using CarParkManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Database.Seeds;

public static class DatabaseSeeder
{
    public static async Task MigrateAndSeedAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarParkManagementDbContext>();
        await dbContext.Database.MigrateAsync();
        await dbContext.SeedAsync();
    }

    private static async Task SeedAsync(this CarParkManagementDbContext dbContext)
    {
        if (dbContext.Cars.Any() || dbContext.ParkingSpaces.Any())
            return;

        List<ParkingSpace> parkingSpaces =[];

        for (int i = 0; i < 50; i++)
        {
            parkingSpaces.Add(new ParkingSpace
            {
                State = ParkingSpaceState.Free,
                OccupiedSince = null,
            });
        }

        await dbContext.ParkingSpaces.AddRangeAsync(parkingSpaces);
        await dbContext.SaveChangesAsync();
    }
}