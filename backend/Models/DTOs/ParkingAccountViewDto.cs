namespace Backend.Models.DTOs;

public class ParkingAccountViewDto
{
    public int Id { get; set; }
    public string ApartmentNumber { get; set; }
    public string FamilyName { get; set; }
    public string Address { get; set; }
    public DateTime? UpdatedAt { get; set; }
}