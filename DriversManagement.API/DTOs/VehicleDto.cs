namespace DriversManagement.API.DTOs;

public class VehicleDto
{
    public int Id { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Engine { get; set; }
    public int FuelCapacity { get; set; }
    public int DriverId { get; set; }
}