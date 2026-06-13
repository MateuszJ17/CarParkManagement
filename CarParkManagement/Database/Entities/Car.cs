namespace CarParkManagement.Database.Entities;

public class Car
{
    public Guid CarId { get; set; }
    public int ParkingSpaceId { get; set; }
    public string RegistrationNumber { get; set; } = null!;
    public int Type { get; set; }
}