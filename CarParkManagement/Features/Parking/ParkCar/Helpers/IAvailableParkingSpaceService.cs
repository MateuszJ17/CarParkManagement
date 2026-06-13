using CarParkManagement.Database.Entities;

namespace CarParkManagement.Features.Parking.ParkCar.Helpers;

public interface IAvailableParkingSpaceService
{
    Task<ParkingSpace?> FindAvailableParkingSpace(CancellationToken cancellationToken = default);
}