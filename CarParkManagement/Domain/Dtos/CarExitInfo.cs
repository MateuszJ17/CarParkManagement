namespace CarParkManagement.Domain.Dtos;

public record CarExitInfo(string VehicleReg, double VehicleCharge, DateTime TimeIn, DateTime TimeOut);