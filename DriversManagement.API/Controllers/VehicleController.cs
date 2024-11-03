using AutoMapper;
using DriversManagement.API.DTOs;
using DriversManagement.API.Interfaces;
using DriversManagement.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DriversManagement.API.Controllers;

[ApiController]
[Route("vehicles")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly IMapper _mapper;

    public VehicleController(IVehicleService vehicleService, IMapper mapper)
    {
        _vehicleService = vehicleService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<VehicleDto>>> GetVehicles(
        [FromQuery] bool isDesc = true,
        [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var vehicles = await _vehicleService.SortVehicles(isDesc, skip, take);
        return Ok(_mapper.Map<ICollection<VehicleDto>>(vehicles));
    }
    
    [HttpGet("vehicle")]
    public async Task<ActionResult<VehicleDto>> GetVehicle(
        [FromQuery] string? model,
        [FromQuery] int? year)
    {
        if (string.IsNullOrEmpty(model) && !year.HasValue)
            return BadRequest("At least one of 'model' or 'year' must be provided.");
        
        var vehicle = await _vehicleService.GetFirstSuitableVehicle(model, year);
        
        if (vehicle == null)
            return NotFound("No vehicle found matching the specified criteria.");
        
        return Ok(_mapper.Map<VehicleDto>(vehicle));
    }
}