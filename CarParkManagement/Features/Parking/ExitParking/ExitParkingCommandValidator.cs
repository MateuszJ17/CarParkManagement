using FluentValidation;

namespace CarParkManagement.Features.Parking.ExitParking;

public class ExitParkingCommandValidator : AbstractValidator<ExitParkingCommand>
{
    public ExitParkingCommandValidator()
    {
        RuleFor(x => x.VehicleReg).NotEmpty().MaximumLength(8);
    }
}