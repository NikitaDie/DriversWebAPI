using DriversManagement.API.Interfaces;
using DriversManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DriversManagement.API.Services;

public class VehicleService : IVehicleService
{
    private readonly IRepository _repository;

    public VehicleService(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ICollection<Vehicle>> SortVehicles(bool isDesc = true, int skip = 0, int take = 10)
    {
        var vehiclesQuery = _repository.GetAll<Vehicle>()
            .AsNoTracking();
        
        vehiclesQuery = isDesc 
            ? vehiclesQuery.OrderByDescending(v => v.Year)
            : vehiclesQuery.OrderBy(v => v.Year);
        
        var sortedVehicles = vehiclesQuery
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();
        
        return await sortedVehicles;
    }

    public async Task<Vehicle?> GetFirstSuitableVehicle(string? model, int? year)
    {
        var vehiclesQuery = _repository.GetAll<Vehicle>()
            .AsNoTracking();

        if (!string.IsNullOrEmpty(model))
            vehiclesQuery = vehiclesQuery.Where(v => v.Model == model);
            

        if (year.HasValue)
            vehiclesQuery = vehiclesQuery.Where(v => v.Year == year);

        var suitableVehicle = vehiclesQuery.FirstOrDefaultAsync();
        
        return await suitableVehicle;
    }
}