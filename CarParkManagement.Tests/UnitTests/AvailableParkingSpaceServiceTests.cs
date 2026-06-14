using CarParkManagement.Database.DbContext;
using CarParkManagement.Domain.Enums;
using CarParkManagement.Features.Parking.ParkCar.Helpers;
using CarParkManagement.Tests.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Tests.UnitTests;

public class AvailableParkingSpaceServiceTests
{
    private AvailableParkingSpaceService _sut;
    
    [Theory]
    [InlineData(1, 0)]
    [InlineData(50, 50)]
    [InlineData(1000, 2000)]
    [InlineData(1, 2000)]
    [InlineData(2, 2)]
    [InlineData(1, 100000)]
    [InlineData(50, 23456)]
    public async Task FindAvailableParkingSpace_Should_ReturnParkingSpace_When_ThereAreAvailableSpaces(
        int freeParkingSpaces,
        int occupiedParkingSpaces)
    {
        var mockParkingSpaces = ParkingSpacesData.PrepareMockParkingSpacesData(freeParkingSpaces, occupiedParkingSpaces);
        
        var options = new DbContextOptionsBuilder<CarParkManagementDbContext>()
            .UseInMemoryDatabase(databaseName: $"FindAvailableParkingSpaceTests_{Guid.NewGuid()}")
            .Options;
        
        await using var inMemoryDbContext = new CarParkManagementDbContext(options);
        await inMemoryDbContext.AddRangeAsync(mockParkingSpaces);
        await inMemoryDbContext.SaveChangesAsync();
        
        _sut = new AvailableParkingSpaceService(inMemoryDbContext);
        
        var result = await _sut.FindAvailableParkingSpace();
        
        Assert.NotNull(result);
        Assert.Equal(ParkingSpaceState.Free, result.State);
    }
}