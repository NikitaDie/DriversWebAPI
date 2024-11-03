namespace DriversManagement.API.DTOs;

public class DriverDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public int CategoryId { get; set; }
    public int Salary { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? LicenceNumber { get; set; }
}