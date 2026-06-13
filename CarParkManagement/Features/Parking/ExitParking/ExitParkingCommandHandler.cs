using CarParkManagement.Database.DbContext;
using CarParkManagement.Domain.Dtos;
using CarParkManagement.Domain.Enums;
using CarParkManagement.Features.Parking.ExitParking.Exceptions;
using CarParkManagement.Features.Parking.ExitParking.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Features.Parking.ExitParking;

public class ExitParkingCommandHandler : IRequestHandler<ExitParkingCommand, CarExitInfo>
{
    private readonly CarParkManagementDbContext _dbContext;
    private readonly IParkingChargeCalculator _parkingChargeCalculator;
    private readonly ILogger<ExitParkingCommandHandler> _logger;

    public ExitParkingCommandHandler(
        CarParkManagementDbContext dbContext,
        IParkingChargeCalculator parkingChargeCalculator,
        ILogger<ExitParkingCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _parkingChargeCalculator = parkingChargeCalculator;
    }

    public async Task<CarExitInfo> Handle(ExitParkingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Exiting parking for vehicle {RequestVehicleReg}", request.VehicleReg);
        
        var parkingSpace = await _dbContext.ParkingSpaces
            .Include(x => x.Car)
            .FirstOrDefaultAsync(x => x.Car!.RegistrationNumber == request.VehicleReg, cancellationToken);

        if (parkingSpace?.Car is null)
        {
            _logger.LogError(
                "Attempted parking exit for vehicle {VehicleReg} but no parking space or car found",
                request.VehicleReg);
            
            throw new ExitParkingException(request.VehicleReg);
        }
        
        _logger.LogInformation(
            "Found parking space {ParkingSpaceParkingSpaceId} for exiting vehicle {RequestVehicleReg}",
            parkingSpace.ParkingSpaceId,
            request.VehicleReg);

        var timeOut = DateTime.UtcNow;
        var parkedCar = parkingSpace.Car;
        var minutesSpentInParking = (timeOut - parkingSpace.OccupiedSince!).Value.TotalMinutes;
        var parkingCharge = _parkingChargeCalculator.CalculateParkingCharge(parkedCar.Type, minutesSpentInParking);
        var timeIn = parkingSpace.OccupiedSince!.Value;
        
        parkingSpace.State = ParkingSpaceState.Free;
        parkingSpace.OccupiedSince = null;
        
        _dbContext.Cars.Remove(parkedCar);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation(
            "Exited parking for vehicle {VehicleReg} with final charge of {ParkingCharge}",
            request.VehicleReg,
            parkingCharge);
        
        return new CarExitInfo(
            parkedCar.RegistrationNumber,
            parkingCharge,
            timeIn,
            timeOut);
    }
}