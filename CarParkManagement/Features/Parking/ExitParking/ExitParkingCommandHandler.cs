using CarParkManagement.Database.DbContext;
using CarParkManagement.Domain.Dtos;
using CarParkManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Features.Parking.ExitParking;

public class ExitParkingCommandHandler : IRequestHandler<ExitParkingCommand, CarExitInfo>
{
    private readonly CarParkManagementDbContext _dbContext;

    public ExitParkingCommandHandler(CarParkManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CarExitInfo> Handle(ExitParkingCommand request, CancellationToken cancellationToken)
    {
        var parkingSpace = await _dbContext.ParkingSpaces
            .Include(x => x.Car)
            .FirstOrDefaultAsync(x => x.Car!.RegistrationNumber == request.VehicleReg, cancellationToken);

        if (parkingSpace is null || parkingSpace.Car is null)
        {
            // todo: throw custom exception
            throw new Exception("No car found");
        }

        var timeOut = DateTime.UtcNow;
        var parkedCar = parkingSpace.Car;
        var minutesSpentInParking = (timeOut - parkingSpace.OccupiedSince!).Value.TotalMinutes;

        var chargePerCarType = parkedCar.Type switch
        {
            1 => 0.10,
            2 => 0.20,
            3 => 0.40,
            _ => throw new ArgumentOutOfRangeException(nameof(parkedCar.Type), "Unknown car type") // todo: throw custom exception
        };

        var normalCharge = minutesSpentInParking * chargePerCarType;
        var fiveMinutesCharge = Math.Floor(minutesSpentInParking / 5);
        
        var timeIn = parkingSpace.OccupiedSince!.Value;
        
        parkingSpace.State = ParkingSpaceState.Free;
        parkingSpace.OccupiedSince = null;
        
        _dbContext.Cars.Remove(parkedCar);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return new CarExitInfo(
            parkedCar.RegistrationNumber,
            normalCharge + fiveMinutesCharge,
            timeIn,
            timeOut);
    }
}