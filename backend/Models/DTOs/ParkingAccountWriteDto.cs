using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DTOs;

public class ParkingAccountWriteDto
{
    [Required]
    public string ApartmentNumber { get; set; }

    [Required]
    public string FamilyName { get; set; }

    [Required]
    public string Address { get; set; }
}