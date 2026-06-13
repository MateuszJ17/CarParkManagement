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

    public ParkCarCommandHandler(CarParkManagementDbContext dbContext, IAvailableParkingSpaceService availableParkingSpaceService)
    {
        _dbContext = dbContext;
        _availableParkingSpaceService = availableParkingSpaceService;
    }

    public async Task<ParkedCarInfo?> Handle(ParkCarCommand request, CancellationToken cancellationToken)
    {
        var availableParkingSpace = await _availableParkingSpaceService.FindAvailableParkingSpace(cancellationToken);
        if (availableParkingSpace is null)
        {
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
        
        return new ParkedCarInfo(
            parkedCar.RegistrationNumber,
            availableParkingSpace.ParkingSpaceId,
            availableParkingSpace.OccupiedSince.Value);
    }
}