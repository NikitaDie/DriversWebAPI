using AutoMapper;
using DriversManagement.API.DTOs;
using DriversManagement.API.Interfaces;
using DriversManagement.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DriversManagement.API.Controllers;

[ApiController]
[Route("drivers")]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;
    private readonly IMapper _mapper;

    public DriverController(IDriverService driverService, IMapper mapper)
    {
        _driverService = driverService;
        _mapper = mapper;
    }

    [HttpGet("filtered-by")]
    public async Task<ActionResult<ICollection<DriverDto>>> GetDrivers(
        [FromQuery] string? firstName,
        [FromQuery] string? lastName,
        [FromQuery] string? licenceNumber,
        [FromQuery] string? searchContext,
        [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var filter = new DriverFilter
        {
            FirstName = firstName,
            LastName = lastName,
            LicenceNumber = licenceNumber,
            SearchContext = searchContext
        };
        var drivers = await _driverService.GetAllFilteredDrivers(filter, skip, take);
        return Ok(_mapper.Map<ICollection<DriverDto>>(drivers));
    }

    [HttpGet]
    public async Task<ActionResult<DriverDto>> GetAllDrivers([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var drivers = await _driverService.GetAllDrivers(skip, take);
        return Ok(_mapper.Map<ICollection<DriverDto>>(drivers));
    }
    
    [HttpGet("by-name")]
    public async Task<ActionResult<DriverDto>> GetDriverByName([FromQuery] string? firstName, [FromQuery] string? lastName)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            return BadRequest("First- and Last- name have to be provided!");
        
        var driver = await _driverService.GetDriverByName(firstName, lastName);
        if (driver == null)
            return NotFound("Driver not found.");

        return Ok(_mapper.Map<DriverDto>(driver));
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<DriverDto>> GetDriverById(int id)
    {
        var driver = await _driverService.GetDriverById(id);
        if (driver == null)
            return NotFound("Driver not found.");

        return Ok(_mapper.Map<DriverDto>(driver));
    }
    
    [HttpPost]
    public async Task<ActionResult<DriverDto>> AddDriver(DriverDto driverDTO)
    {
        var driver = _mapper.Map<Driver>(driverDTO);
        var createdDriver = await _driverService.AddDriver(driver);
        return CreatedAtAction(nameof(GetDriverById), new { id = createdDriver.Id }, _mapper.Map<DriverDto>(createdDriver));
    }
    
    [HttpPatch("{id}/update-name")]
    public async Task<IActionResult> UpdateName(int id, [FromQuery] string? newFirstName, [FromQuery] string? newLastName)
    {
        if (string.IsNullOrEmpty(newFirstName) || string.IsNullOrEmpty(newLastName))
            return BadRequest("First- and Last- name have to be provided!");
        
        var updatedDriver = await _driverService.UpdateDriverName(id, newFirstName, newLastName);
        if (updatedDriver == null)
            return NotFound("Driver not found.");

        return Ok(_mapper.Map<DriverDto>(updatedDriver));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDriver(int id, DriverDto driverDto)
    {
        var driver = _mapper.Map<Driver>(driverDto);
        var updatedDriver = await _driverService.UpdateDriver(id, driver);
        if (updatedDriver == null)
            return NotFound("Driver not found.");

        return Ok(_mapper.Map<DriverDto>(updatedDriver));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriver(int id)
    {
        var result = await _driverService.DeleteDriver(id);
        if (!result)
            return NotFound("Driver not found.");

        return NoContent();
    }
}