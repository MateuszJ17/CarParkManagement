using CarParkManagement.Domain.Enums;

namespace CarParkManagement.Database.Entities;

public class ParkingSpace
{
    public int ParkingSpaceId { get; set; }
    public ParkingSpaceState State { get; set; }
    public DateTime? OccupiedSince { get; set; }
    
    public Car? Car { get; set; }
}