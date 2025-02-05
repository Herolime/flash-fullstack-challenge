using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using Backend.Models.SearchParams;
using Backend.Models.DTOs;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<VehicleViewDto>> Get([FromQuery] VehicleSearchParams searchParams)
    {
        var vehicles = _vehicleService.Search(searchParams);
        return Ok(vehicles);
    }

    [HttpGet("{id}")]
    public ActionResult<VehicleViewDto> GetById(int id)
    {
        var vehicle = _vehicleService.GetById(id);
        if (vehicle == null)
            return NotFound();

        return Ok(vehicle);
    }

    [HttpPost]
    public ActionResult<VehicleViewDto> Create(VehicleWriteDto dto)
    {
        var createdVehicle = _vehicleService.Create(new Vehicle
        {
            AccountId = dto.AccountId,
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year,
            LicensePlate = dto.LicensePlate,
            Color = dto.Color,
            Type = dto.Type,
        });
        return CreatedAtAction(nameof(GetById), new { id = createdVehicle.Id }, createdVehicle);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, VehicleWriteDto dto)
    {
        var existingVehicle = _vehicleService.GetById(id);
        if (existingVehicle == null)
            return NotFound();

        var updatedVehicle = new Vehicle
        {
            Id = existingVehicle.Id,
            AccountId = existingVehicle.AccountId,
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year,
            LicensePlate = dto.LicensePlate,
            Color = dto.Color,
            Type = dto.Type,
        };
        _vehicleService.Update(id, updatedVehicle);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existingVehicle = _vehicleService.GetById(id);
        if (existingVehicle == null)
            return NotFound();

        _vehicleService.Delete(id);
        return NoContent();
    }
}