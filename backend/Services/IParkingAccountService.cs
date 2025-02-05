using Backend.Models;
using Backend.Models.SearchParams;

namespace Backend.Services;

public interface IParkingAccountService
{
    IEnumerable<ParkingAccount> Search(ParkingAccountSearchParams searchParams);
    ParkingAccount? GetById(int id);
    ParkingAccount Create(ParkingAccount account);
    void Update(int id, ParkingAccount account);
    void Delete(int id);
    IEnumerable<Contact> GetContacts(int Id);
    IEnumerable<Vehicle> GetVehicles(int Id);
}