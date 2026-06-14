using CarParkManagement.Database.DbContext;
using CarParkManagement.Database.Entities;
using CarParkManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Features.Parking.ParkCar.Helpers;

public class AvailableParkingSpaceService : IAvailableParkingSpaceService
{
    private readonly CarParkManagementDbContext _dbContext;

    public AvailableParkingSpaceService(CarParkManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ParkingSpace?> FindAvailableParkingSpace(CancellationToken cancellationToken = default)
    {
        var availableParkingSpace = await _dbContext.ParkingSpaces
            .Where(x => x.State == ParkingSpaceState.Free)
            .OrderBy(x => x.ParkingSpaceId)
            .FirstOrDefaultAsync(cancellationToken);
        
        return availableParkingSpace;
    }
}