namespace Backend.Models.DTOs;

public class VehicleViewDto
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public string? Color { get; set; }
    public string? Type { get; set; }
    public DateTime? UpdatedAt { get; set; }
}