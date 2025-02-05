using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using Backend.Models.SearchParams;
using Backend.Models.DTOs;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ContactViewDto>> Get([FromQuery] ContactSearchParams searchParams)
    {
        var contacts = _contactService.Search(searchParams);
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public ActionResult<ContactViewDto> GetById(int id)
    {
        var contact = _contactService.GetById(id);
        if (contact == null)
            return NotFound();

        return Ok(contact);
    }

    [HttpPost]
    public ActionResult<ContactViewDto> Create(ContactWriteDto dto)
    {
        var createdContact = _contactService.Create(new Contact
        {
            AccountId = dto.AccountId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        });
        return CreatedAtAction(nameof(GetById), new { id = createdContact.Id }, createdContact);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, ContactWriteDto dto)
    {
        var existingContact = _contactService.GetById(id);
        if (existingContact == null)
            return NotFound();

        var updatedContact = new Contact
        {
            Id = existingContact.Id,
            AccountId = existingContact.AccountId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };
        _contactService.Update(id, updatedContact);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existingContact = _contactService.GetById(id);
        if (existingContact == null)
            return NotFound();

        _contactService.Delete(id);
        return NoContent();
    }
}