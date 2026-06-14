using CarParkManagement.Features.Parking.ExitParking.Exceptions;

namespace CarParkManagement.Features.Parking.ExitParking.Helpers;

public class ParkingChargeCalculator : IParkingChargeCalculator
{
    public double CalculateParkingCharge(
        int parkedCarType,
        double minutesSpentInParking)
    {
        var chargePerCarType = parkedCarType switch
        {
            1 => 0.10,
            2 => 0.20,
            3 => 0.40,
            _ => throw new UnknownCarTypeException(parkedCarType)
        };

        var normalCharge = minutesSpentInParking * chargePerCarType;
        var fiveMinutesCharge = Math.Floor(minutesSpentInParking / 5);
        
        return Math.Round(normalCharge + fiveMinutesCharge, 2);
    }
}