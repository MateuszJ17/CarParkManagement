using CarParkManagement.Domain.Dtos;
using MediatR;

namespace CarParkManagement.Features.Parking.ExitParking;

public record ExitParkingCommand(string VehicleReg) : IRequest<CarExitInfo>;