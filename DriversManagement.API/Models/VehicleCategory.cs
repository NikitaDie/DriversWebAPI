namespace DriversManagement.API.Models;

public class VehicleCategory
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public int Age { get; set; }
    public int Mass { get; set; }

    public ICollection<Driver> Drivers { get; set; }
}

