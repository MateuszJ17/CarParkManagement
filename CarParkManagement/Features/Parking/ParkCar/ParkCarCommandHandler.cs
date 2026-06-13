using CarParkManagement.Database.DbContext;
using CarParkManagement.Database.Entities;
using CarParkManagement.Domain.Dtos;
using CarParkManagement.Domain.Enums;
using CarParkManagement.Features.Parking.ParkCar.Helpers;
using MediatR;

namespace CarParkManagement.Features.Parking.ParkCar;

public class ParkCarCommandHandler : IRequestHandler<ParkCarCommand, ParkedCarInfo?>
{
    private readonly CarParkManagementDbContext _dbContext;
    private readonly IAvailableParkingSpaceService _availableParkingSpaceService;
    private readonly ILogger<ParkCarCommandHandler> _logger;

    public ParkCarCommandHandler(
        CarParkManagementDbContext dbContext,
        IAvailableParkingSpaceService availableParkingSpaceService,
        ILogger<ParkCarCommandHandler> logger)
    {
        _dbContext = dbContext;
        _availableParkingSpaceService = availableParkingSpaceService;
        _logger = logger;
    }

    public async Task<ParkedCarInfo?> Handle(ParkCarCommand request, CancellationToken cancellationToken)
    {
        var availableParkingSpace = await _availableParkingSpaceService.FindAvailableParkingSpace(cancellationToken);
        if (availableParkingSpace is null)
        {
            _logger.LogWarning("No available parking spaces found for vehicle {RequestVehicleReg}", request.VehicleReg);
            return null;
        }

        var parkedCar = new Car
        {
            ParkingSpaceId = availableParkingSpace.ParkingSpaceId,
            RegistrationNumber = request.VehicleReg,
            Type = request.VehicleType
        };
        await _dbContext.Cars.AddAsync(parkedCar, cancellationToken);
        
        availableParkingSpace.Car = parkedCar;
        availableParkingSpace.State = ParkingSpaceState.Occupied;
        availableParkingSpace.OccupiedSince = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation(
            "Parked vehicle {RequestVehicleReg} in parking space {ParkingSpaceParkingSpaceId}",
            request.VehicleReg,
            availableParkingSpace.ParkingSpaceId);
        
        return new ParkedCarInfo(
            parkedCar.RegistrationNumber,
            availableParkingSpace.ParkingSpaceId,
            availableParkingSpace.OccupiedSince.Value);
    }
}