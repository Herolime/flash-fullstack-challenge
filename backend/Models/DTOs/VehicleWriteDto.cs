using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DTOs;

public class VehicleWriteDto
{
    public int AccountId { get; set; }

    [Required]
    public string Make { get; set; }

    [Required]
    public string Model { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public string LicensePlate { get; set; }

    public string? Color { get; set; }
    public string? Type { get; set; }
}