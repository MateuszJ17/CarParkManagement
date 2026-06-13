namespace CarParkManagement.Features.Parking.ExitParking.Exceptions;

public class UnknownCarTypeException : Exception
{
    public UnknownCarTypeException(int carType) : base($"Unknown car type {carType}")
    {
    }
}