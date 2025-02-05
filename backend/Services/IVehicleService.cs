using Backend.Models;
using Backend.Models.SearchParams;

namespace Backend.Services;

public interface IVehicleService
{
    IEnumerable<Vehicle> Search(VehicleSearchParams searchParams);
    Vehicle? GetById(int id);
    Vehicle Create(Vehicle vehicle);
    void Update(int id, Vehicle vehicle);
    void Delete(int id);
}