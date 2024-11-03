using DriversManagement.API.Models;
namespace DriversManagement.API.Interfaces;

public interface IVehicleService
{
    Task<ICollection<Vehicle>> SortVehicles(bool isDesc, int skip, int take);
    Task<Vehicle?> GetFirstSuitableVehicle(string? model, int? year);
}