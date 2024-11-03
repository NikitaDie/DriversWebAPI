using DriversManagement.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DriversManagement.API.Interfaces;

public interface IDriverService
{
    Task<ICollection<Driver>> GetAllDrivers(int skip, int take);
    Task<ICollection<Driver>> GetAllFilteredDrivers(DriverFilter filter, int skip, int take);
    Task<ICollection<Vehicle>> FilterVehicles(string? model, int? year, string? driverFirstName);
    
    Task<Driver?> GetDriverByName(string firstName, string lastName);
    
    Task<Driver?> GetDriverById(int id);
    
    Task<Driver> AddDriver(Driver driver);
    
    Task<Driver?> UpdateDriverName(int id, string newFirstName, string newLastName);
    
    Task<Driver?> UpdateDriver(int id, Driver updatedDriver);
    
    Task<bool> DeleteDriver(int id);
}