using CarParkManagement.Domain.Dtos;
using MediatR;

namespace CarParkManagement.Features.Parking.FetchParkingSpaces;

public record FetchParkingSpacesQuery() : IRequest<AvailableSpacesInfo>;
