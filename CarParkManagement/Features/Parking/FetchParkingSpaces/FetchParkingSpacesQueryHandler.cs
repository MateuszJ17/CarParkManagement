using CarParkManagement.Domain.Dtos;
using CarParkManagement.Features.Parking.FetchParkingSpaces.Helpers;
using MediatR;

namespace CarParkManagement.Features.Parking.FetchParkingSpaces;

public class FetchParkingSpacesQueryHandler : IRequestHandler<FetchParkingSpacesQuery, AvailableSpacesInfo>
{
    private readonly IParkingSpacesInfoService _parkingSpacesInfoService;

    public FetchParkingSpacesQueryHandler(IParkingSpacesInfoService parkingSpaceFinder)
    {
        _parkingSpacesInfoService = parkingSpaceFinder;
    }

    public async Task<AvailableSpacesInfo> Handle(FetchParkingSpacesQuery request, CancellationToken cancellationToken)
    {
        return await _parkingSpacesInfoService.GetAllParkingSpacesSummary(cancellationToken);
    }
}