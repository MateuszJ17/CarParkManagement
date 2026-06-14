using CarParkManagement.Domain.Dtos;

namespace CarParkManagement.Features.Parking.FetchParkingSpaces.Helpers;

public interface IParkingSpacesInfoService
{
    Task<AvailableSpacesInfo> GetAllParkingSpacesSummary(CancellationToken cancellationToken = default);
}