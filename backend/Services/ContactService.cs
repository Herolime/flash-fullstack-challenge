using Backend.Models;
using Backend.Models.SearchParams;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Backend.Services;

public class ContactService : IContactService
{
    private readonly IDbConnection _db;

    public ContactService(IDbConnection db)
    {
        _db = db;
    }

    public IEnumerable<Contact> Search(ContactSearchParams searchParams)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        var conditions = new List<string>
        {
            "status = 'Active'"
        };

        if (searchParams.AccountId.HasValue)
        {
            conditions.Add("account_id = @AccountId");
            command.Parameters.Add(new SqliteParameter("@AccountId", searchParams.AccountId.Value));
        }

        if (!string.IsNullOrEmpty(searchParams.FirstName))
        {
            conditions.Add("first_name LIKE @FirstName");
            command.Parameters.Add(new SqliteParameter("@FirstName", $"%{searchParams.FirstName}%"));
        }

        if (!string.IsNullOrEmpty(searchParams.LastName))
        {
            conditions.Add("last_name LIKE @LastName");
            command.Parameters.Add(new SqliteParameter("@LastName", $"%{searchParams.LastName}%"));
        }

        if (!string.IsNullOrEmpty(searchParams.Email))
        {
            conditions.Add("email LIKE @Email");
            command.Parameters.Add(new SqliteParameter("@Email", $"%{searchParams.Email}%"));
        }

        if (!string.IsNullOrEmpty(searchParams.PhoneNumber))
        {
            conditions.Add("phone_number LIKE @PhoneNumber");
            command.Parameters.Add(new SqliteParameter("@PhoneNumber", $"%{searchParams.PhoneNumber}%"));
        }

        var whereClause = conditions.Count > 0 ? $" WHERE {string.Join(" AND ", conditions)}" : string.Empty;
        command.CommandText = $"SELECT * FROM contacts{whereClause}";

        try
        {
            using var reader = command.ExecuteReader();
            var contacts = new List<Contact>();

            while (reader.Read())
            {
                contacts.Add(new Contact
                {
                    Id = reader.GetInt32(0),
                    AccountId = reader.GetInt32(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    Email = reader.GetString(4),
                    PhoneNumber = reader.GetString(5),
                    CreatedAt = reader.GetDateTime(6),
                    UpdatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7)
                });
            }

            return contacts;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            _db.Close();
        }
    }

    public Contact? GetById(int id)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = "SELECT * FROM contacts WHERE status = 'Active' AND id = @Id";

        try
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Contact
            {
                Id = reader.GetInt32(0),
                AccountId = reader.GetInt32(1),
                FirstName = reader.GetString(2),
                LastName = reader.GetString(3),
                Email = reader.GetString(4),
                PhoneNumber = reader.GetString(5),
                CreatedAt = reader.GetDateTime(6),
                UpdatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7)
            };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            _db.Close();
        }
    }

    public Contact Create(Contact contact)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = @"INSERT INTO contacts 
                (account_id, first_name, last_name, email, phone_number, created_at) 
                VALUES (@AccountId, @FirstName, @LastName, @Email, @PhoneNumber, @CreatedAt);
                SELECT last_insert_rowid();";
        try
        {
            command.Parameters.Add(new SqliteParameter("@AccountId", contact.AccountId));
            command.Parameters.Add(new SqliteParameter("@FirstName", contact.FirstName));
            command.Parameters.Add(new SqliteParameter("@LastName", contact.LastName));
            command.Parameters.Add(new SqliteParameter("@Email", contact.Email));
            command.Parameters.Add(new SqliteParameter("@PhoneNumber", contact.PhoneNumber));
            command.Parameters.Add(new SqliteParameter("@CreatedAt", DateTime.UtcNow));

            contact.Id = Convert.ToInt32(command.ExecuteScalar());
            contact.CreatedAt = DateTime.UtcNow;
            return contact;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            _db.Close();
        }
    }

    public void Update(int id, Contact contact)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = @"UPDATE contacts 
            SET account_id = @AccountId, 
                first_name = @FirstName, 
                last_name = @LastName, 
                email = @Email, 
                phone_number = @PhoneNumber, 
                updated_at = @UpdatedAt 
            WHERE id = @Id";

        try
        {
            command.Parameters.Add(new SqliteParameter("@Id", id));
            command.Parameters.Add(new SqliteParameter("@AccountId", contact.AccountId));
            command.Parameters.Add(new SqliteParameter("@FirstName", contact.FirstName));
            command.Parameters.Add(new SqliteParameter("@LastName", contact.LastName));
            command.Parameters.Add(new SqliteParameter("@Email", contact.Email));
            command.Parameters.Add(new SqliteParameter("@PhoneNumber", contact.PhoneNumber));
            command.Parameters.Add(new SqliteParameter("@UpdatedAt", DateTime.UtcNow));

            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            _db.Close();
        }
    }

    public void Delete(int id)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = @"UPDATE contacts 
            SET status = 'Inactive',
            updated_at = @UpdatedAt
            WHERE id = @Id";
        try
        {
            command.Parameters.Add(new SqliteParameter("@Id", id));
            command.Parameters.Add(new SqliteParameter("@UpdatedAt", DateTime.UtcNow));
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            _db.Close();
        }
    }
}