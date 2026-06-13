using FluentValidation;

namespace CarParkManagement.Features.Parking.ParkCar;

public class ParkCarCommandValidator : AbstractValidator<ParkCarCommand>
{
    public ParkCarCommandValidator()
    {
        RuleFor(x => x.VehicleReg).NotEmpty().MaximumLength(8);
        RuleFor(x => x.VehicleType).GreaterThan(0).LessThanOrEqualTo(3);
    }
}