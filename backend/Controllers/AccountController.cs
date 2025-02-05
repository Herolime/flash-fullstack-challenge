using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using Backend.Models.SearchParams;
using Backend.Models.DTOs;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IParkingAccountService _accountService;

    public AccountController(IParkingAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ParkingAccountViewDto>> Get([FromQuery] ParkingAccountSearchParams searchParams)
    {
        var accounts = _accountService.Search(searchParams);
        return Ok(accounts);
    }

    [HttpGet("{id}")]
    public ActionResult<ParkingAccountViewDto> GetById(int id)
    {
        var account = _accountService.GetById(id);
        if (account == null)
            return NotFound();

        return Ok(account);
    }

    [HttpGet("{id}/contacts")]
    public ActionResult<IEnumerable<Contact>> GetContacts(int id)
    {
        var account = _accountService.GetById(id);
        if (account == null)
            return NotFound();

        var contacts = _accountService.GetContacts(id);
        return Ok(contacts);
    }

    [HttpGet("{id}/vehicles")]
    public ActionResult<IEnumerable<Vehicle>> GetVehicles(int id)
    {
        var account = _accountService.GetById(id);
        if (account == null)
            return NotFound();

        var vehicles = _accountService.GetVehicles(id);
        return Ok(vehicles);
    }

    [HttpPost]
    public ActionResult<ParkingAccount> Create(ParkingAccountWriteDto dto)
    {
        var createdAccount = _accountService.Create(new ParkingAccount
        {
            ApartmentNumber = dto.ApartmentNumber,
            FamilyName = dto.FamilyName,
            Address = dto.Address,
        });
        return CreatedAtAction(nameof(GetById), new { id = createdAccount.Id }, createdAccount);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, ParkingAccountWriteDto dto)
    {
        var existingAccount = _accountService.GetById(id);
        if (existingAccount == null)
            return NotFound();

        var updatedAccount = new ParkingAccount
        {
            Id = existingAccount.Id,
            ApartmentNumber = dto.ApartmentNumber,
            FamilyName = dto.FamilyName,
            Address = dto.Address
        };
        _accountService.Update(id, updatedAccount);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existingAccount = _accountService.GetById(id);
        if (existingAccount == null)
            return NotFound();

        _accountService.Delete(id);
        return NoContent();
    }
}