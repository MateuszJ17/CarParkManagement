using CarParkManagement.Database.DbContext;
using CarParkManagement.Domain.Dtos;
using CarParkManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Features.Parking.FetchParkingSpaces.Helpers;

public class ParkingSpacesInfoService : IParkingSpacesInfoService
{
    private readonly CarParkManagementDbContext _dbContext;

    public ParkingSpacesInfoService(CarParkManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AvailableSpacesInfo> GetAllParkingSpacesSummary(CancellationToken cancellationToken = default)
    {
        var availableSpaces = await _dbContext.ParkingSpaces
            .AsNoTracking()
            .CountAsync(x => x.State == ParkingSpaceState.Free, cancellationToken);
        
        var occupiedSpaces = await _dbContext.ParkingSpaces
            .AsNoTracking()
            .CountAsync(x => x.State == ParkingSpaceState.Occupied, cancellationToken);

        return new AvailableSpacesInfo(availableSpaces, occupiedSpaces);
    }
}