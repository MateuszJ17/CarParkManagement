using CarParkManagement.Database.DbContext;
using CarParkManagement.Features.Parking.FetchParkingSpaces.Helpers;
using CarParkManagement.Tests.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Tests.UnitTests;

public class ParkingSpacesInfoServiceTests
{
    private ParkingSpacesInfoService _sut;
    
    [Theory]
    [InlineData(1, 0)]
    [InlineData(50, 50)]
    [InlineData(344, 352)]
    [InlineData(10, 11)]
    [InlineData(6, 6)]
    public async Task GetAllParkingSpacesSummary_Should_ReturnCorrectSummary(int availableParkingSpaces, int occupiedParkingSpaces)
    {
        var mockParkingSpaces = ParkingSpacesData.PrepareMockParkingSpacesData(availableParkingSpaces, occupiedParkingSpaces);
        
        var options = new DbContextOptionsBuilder<CarParkManagementDbContext>()
            .UseInMemoryDatabase(databaseName: $"GetAllParkingSpacesSummaryTests_{Guid.NewGuid()}")
            .Options;

        await using var inMemoryDbContext = new CarParkManagementDbContext(options);
        await inMemoryDbContext.AddRangeAsync(mockParkingSpaces);
        await inMemoryDbContext.SaveChangesAsync();
        
        _sut  = new ParkingSpacesInfoService(inMemoryDbContext);
        
        var result = await _sut.GetAllParkingSpacesSummary();
        
        Assert.Equal(availableParkingSpaces, result.AvailableSpaces);
        Assert.Equal(occupiedParkingSpaces, result.OccupiedSpaces);
    }
}