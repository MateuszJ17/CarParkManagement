using CarParkManagement.Domain.Dtos;
using MediatR;

namespace CarParkManagement.Features.Parking.ParkCar;

public record ParkCarCommand(string VehicleReg, int VehicleType) : IRequest<ParkedCarInfo?>;