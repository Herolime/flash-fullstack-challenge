using Backend.Models;
using Backend.Models.SearchParams;

namespace Backend.Services;

public interface IContactService
{
    IEnumerable<Contact> Search(ContactSearchParams searchParams);
    Contact? GetById(int id);
    Contact Create(Contact contact);
    void Update(int id, Contact contact);
    void Delete(int id);
}