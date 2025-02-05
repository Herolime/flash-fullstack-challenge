using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DTOs;

public class ContactWriteDto
{
    public int AccountId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }
}