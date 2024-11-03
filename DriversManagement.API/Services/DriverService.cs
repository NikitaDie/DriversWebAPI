using DriversManagement.API.Interfaces;
using DriversManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DriversManagement.API.Services;

public class DriverService : IDriverService
{
    private readonly IRepository _repository;

    public DriverService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<Driver>> GetAllDrivers(int skip, int take)
    {
        return await _repository.GetAll<Driver>()
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();
    }
    
    public async Task<ICollection<Driver>> GetAllFilteredDrivers(DriverFilter filter, int skip, int take)
    {
        // hardcode
        filter.CategoryIds = [2, 4, 5];
        string[] categories = ["A", "B", "T"];
        var categoryHashSet = categories.ToHashSet();
        
        var drivers = _repository.GetAll<Driver>()
            .Include(d => d.Category)
            .AsQueryable();

        if (filter.SearchContext != null)
        {
            drivers = drivers.Where(d =>
                (d.FirstName + " " + d.LastName + " " + d.DateOfBirth + " " + d.LicenceNumber + " " +
                 d.Category).Contains(filter.SearchContext));
        }
        else
        {
            if (filter.FirstName != null)
            {
                // SQL: where [d].FirstName like '%'+@filteName+'%'
                drivers = drivers.Where(d => d.FirstName.Contains(filter.FirstName));
            }

            if (filter.LastName != null)
            {
                drivers = drivers.Where(d => d.LastName.Contains(filter.LastName));
            }

            if (filter.LicenceNumber != null)
            {
                drivers = drivers.Where(d => d.LicenceNumber.StartsWith(filter.LicenceNumber));
            }
        }
        
        var ids = _repository.GetAll<VehicleCategory>()
            .Where(x => categoryHashSet.Contains(x.Symbol))
            .Select(x => x.Id);
        drivers = drivers.Where(d => ids.Contains(d.Category.Id));

        drivers = drivers.OrderBy(d => d.LastName).ThenBy(d => d.FirstName);
            
        return await drivers.Skip(skip)
            .Take(take)
            .ToArrayAsync();
    }

    public async Task<ICollection<Vehicle>> FilterVehicles(string? model, int? year, string? driverFirstName)
    {
        var vehicles = _repository.GetAll<Vehicle>()
            .Include(d => d.Driver)
            .AsNoTracking()
            .AsQueryable();
        
        var validCategories = new HashSet<string> { "A", "B", "B1" };
        
        if (vehicles.Any(v => v.Driver != null && validCategories.Contains(v.Driver.Category.Symbol)))
        {
            vehicles = vehicles.Where(v => v.FuelCapacity < 30);
        }
        
        if (!string.IsNullOrEmpty(model))
        {
            vehicles = vehicles.Where(v => v.Model == model);
        }

        if (year.HasValue)
        {
            vehicles = vehicles.Where(v => v.Year == year.Value);
        }

        vehicles = string.IsNullOrEmpty(driverFirstName) 
            ? vehicles.Where(v => v.Driver != null && v.Driver.Category != null) 
            : vehicles.Where(v => v.Driver.FirstName == driverFirstName);

        return await vehicles.ToArrayAsync();
    }
    
    public async Task<Driver?> GetDriverByName(string firstName, string lastName)
    {
        return await _repository.GetAll<Driver>()
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.FirstName == firstName && d.LastName == lastName);
    }

    public async Task<Driver?> GetDriverById(int id)
    {
        return await _repository.GetAll<Driver>()
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Driver> AddDriver(Driver driver)
    {
        var vehicleCategory = await _repository.GetAll<VehicleCategory>()
            .FirstOrDefaultAsync(v => v.Id == driver.CategoryId);

        if (vehicleCategory == null)
        {
            throw new ArgumentException("Vehicle category not found.");
        }

        driver.Category = vehicleCategory;
    
        await _repository.AddAsync(driver);
        await _repository.SaveChangesAsync();

        return driver;
    }
    
    public async Task<Driver?> UpdateDriverName(int id, string newFirstName, string newLastName)
    {
        var driver = await GetDriverById(id);
        if (driver == null) return null;

        driver.FirstName = newFirstName;
        driver.LastName = newLastName;

        _repository.Update(driver);
        await _repository.SaveChangesAsync();
        
        return driver;
    }
    
    public async Task<Driver?> UpdateDriver(int id, Driver updatedDriver)
    {
        var driver = await GetDriverById(id);
        if (driver == null) return null;

        var vehicleCategory = await _repository.GetAll<VehicleCategory>()
            .FirstOrDefaultAsync(v => v.Id == driver.CategoryId);

        if (vehicleCategory == null)
        {
            throw new ArgumentException("Vehicle category not found.");
        }
        
        driver.FirstName = updatedDriver.FirstName;
        driver.LastName = updatedDriver.LastName;
        driver.DateOfBirth = updatedDriver.DateOfBirth;
        driver.LicenceNumber = updatedDriver.LicenceNumber;
        driver.Salary = updatedDriver.Salary;
        driver.Category = vehicleCategory;
        driver.CategoryId = updatedDriver.CategoryId;

        _repository.Update(driver);
        await _repository.SaveChangesAsync();
        
        return driver;
    }
    
    public async Task<bool> DeleteDriver(int id)
    {
        var driver = await GetDriverById(id);
        if (driver == null) return false;

        _repository.Delete(driver);
        await _repository.SaveChangesAsync();
        
        return true;
    }
}