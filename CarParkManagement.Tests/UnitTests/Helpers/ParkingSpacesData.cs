using CarParkManagement.Database.Entities;
using CarParkManagement.Domain.Enums;

namespace CarParkManagement.Tests.UnitTests.Helpers;

public static class ParkingSpacesData
{
    public static List<ParkingSpace> PrepareMockParkingSpacesData(int availableParkingSpaces, int occupiedParkingSpaces)
    {
        var result = new List<ParkingSpace>();
        int lastParkingSpaceId = 0;

        for (int i = 0; i < availableParkingSpaces; i++)
        {
            result.Add(new ParkingSpace
            {
                ParkingSpaceId = lastParkingSpaceId + 1,
                State = ParkingSpaceState.Free
            });
            lastParkingSpaceId++;
        }
        
        for (int i = 0; i < occupiedParkingSpaces; i++)
        {
            result.Add(new ParkingSpace
            {
                ParkingSpaceId = lastParkingSpaceId + 1,
                State = ParkingSpaceState.Occupied
            });
            lastParkingSpaceId++;
        }
        
        return result;
    }
}