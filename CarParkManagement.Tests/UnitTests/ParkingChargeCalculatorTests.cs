using CarParkManagement.Features.Parking.ExitParking.Exceptions;
using CarParkManagement.Features.Parking.ExitParking.Helpers;

namespace CarParkManagement.Tests.UnitTests;

public class ParkingChargeCalculatorTests
{
    private readonly ParkingChargeCalculator _sut = new ParkingChargeCalculator();

    [Theory]
    [InlineData(1, 10, 3)]
    [InlineData(1, 15, 4.5)]
    [InlineData(2, 60, 24)]
    [InlineData(2, 100, 40)]
    [InlineData(3, 200, 120)]
    [InlineData(3, 600, 360)]
    [InlineData(1, 13, 3.3)]
    [InlineData(2, 22, 8.4)]
    [InlineData(3, 87, 51.8)]
    public void CalculateParkingCharge_Should_ReturnCorrectCharge_ForCorrectCarTypeAndMinutesSpent(
        int parkedCarType,
        double minutesSpentInParking,
        double expectedCharge)
    {
        var result = _sut.CalculateParkingCharge(parkedCarType, minutesSpentInParking);
        
        Assert.Equal(expectedCharge, result);
    }

    [Fact]
    public void CalculateParkingCharge_Should_ThrowException_ForUnknownCarType()
    {
        int incorrectCarType = 5;
        
        Assert.Throws<UnknownCarTypeException>(() => _sut.CalculateParkingCharge(incorrectCarType, 10));
    }
}