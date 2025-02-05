namespace Backend.Models.SearchParams;

public class VehicleSearchParams
{
    public int? AccountId { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public string? LicensePlate { get; set; }
    public string? Color { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
}