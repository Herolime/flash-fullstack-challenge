namespace Backend.Models.DTOs;

public class ContactViewDto
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? UpdatedAt { get; set; }
}