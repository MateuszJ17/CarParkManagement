namespace CarParkManagement.Features.Parking.ExitParking.Helpers;

public interface IParkingChargeCalculator
{
    double CalculateParkingCharge(
        int parkedCarType,
        double minutesSpentInParking);
}