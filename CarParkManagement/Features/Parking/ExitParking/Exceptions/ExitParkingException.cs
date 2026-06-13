namespace CarParkManagement.Features.Parking.ExitParking.Exceptions;

public class ExitParkingException : Exception
{
    public ExitParkingException(string vehicleReg)
        : base($"Vehicle {vehicleReg} or it's parking space not found in parking while exiting")
    {
    }
}