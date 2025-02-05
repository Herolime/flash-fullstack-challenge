using Backend.Models;
using Backend.Models.SearchParams;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Backend.Services;

public class ParkingAccountService : IParkingAccountService
{
    private readonly IDbConnection _db;

    public ParkingAccountService(IDbConnection db)
    {
        _db = db;
    }

    public IEnumerable<ParkingAccount> Search(ParkingAccountSearchParams searchParams)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        var conditions = new List<string>
        {
            "status = 'Active'"
        };

        if (!string.IsNullOrEmpty(searchParams.ApartmentNumber))
        {
            conditions.Add("apartment_number LIKE @ApartmentNumber");
            command.Parameters.Add(new SqliteParameter("@ApartmentNumber", $"%{searchParams.ApartmentNumber}%"));
        }

        if (!string.IsNullOrEmpty(searchParams.FamilyName))
        {
            conditions.Add("family_name LIKE @FamilyName");
            command.Parameters.Add(new SqliteParameter("@FamilyName", $"%{searchParams.FamilyName}%"));
        }

        var whereClause = conditions.Count > 0 ? $" WHERE {string.Join(" AND ", conditions)}" : string.Empty;
        command.CommandText = $"SELECT * FROM parking_accounts{whereClause}";

        try
        {
            using var reader = command.ExecuteReader();
            var accounts = new List<ParkingAccount>();

            while (reader.Read())
            {
                accounts.Add(new ParkingAccount
                {
                    Id = reader.GetInt32(0),
                    ApartmentNumber = reader.GetString(1),
                    FamilyName = reader.GetString(2),
                    Address = reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4),
                    UpdatedAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5)
                });
            }

            return accounts;
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

    public ParkingAccount? GetById(int id)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = "SELECT * FROM parking_accounts WHERE status = 'Active' AND id = @Id";

        try
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new ParkingAccount
            {
                Id = reader.GetInt32(0),
                ApartmentNumber = reader.GetString(1),
                FamilyName = reader.GetString(2),
                Address = reader.GetString(3),
                CreatedAt = reader.GetDateTime(4),
                UpdatedAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5)
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

    public ParkingAccount Create(ParkingAccount account)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = @"INSERT INTO parking_accounts 
            (apartment_number, family_name, address, created_at) 
            VALUES (@ApartmentNumber, @FamilyName, @Address, @CreatedAt);
            SELECT last_insert_rowid();";

        try
        {
            command.Parameters.Add(new SqliteParameter("@ApartmentNumber", account.ApartmentNumber));
            command.Parameters.Add(new SqliteParameter("@FamilyName", account.FamilyName));
            command.Parameters.Add(new SqliteParameter("@Address", account.Address));
            command.Parameters.Add(new SqliteParameter("@CreatedAt", DateTime.UtcNow));

            account.Id = Convert.ToInt32(command.ExecuteScalar());
            account.CreatedAt = DateTime.UtcNow;
            return account;
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

    public void Update(int id, ParkingAccount account)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = @"UPDATE parking_accounts 
            SET apartment_number = @ApartmentNumber, 
                family_name = @FamilyName, 
                address = @Address, 
                updated_at = @UpdatedAt 
            WHERE id = @Id";

        try
        {
            command.Parameters.Add(new SqliteParameter("@Id", id));
            command.Parameters.Add(new SqliteParameter("@ApartmentNumber", account.ApartmentNumber));
            command.Parameters.Add(new SqliteParameter("@FamilyName", account.FamilyName));
            command.Parameters.Add(new SqliteParameter("@Address", account.Address));
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
        try
        {
            // check for related properties, since they need to be marked inactive as well
            var contacts = GetContacts(id);
            var vehicles = GetVehicles(id);
            if (contacts.Any())
            {
                using var contactCommand = _db.CreateCommand();
                contactCommand.CommandText = @"UPDATE contacts
                    SET status = 'Inactive',
                    updated_at = @UpdatedAt
                    WHERE account_id = @Id";
                contactCommand.Parameters.Add(new SqliteParameter("@Id", id));
                contactCommand.Parameters.Add(new SqliteParameter("@UpdatedAt", DateTime.UtcNow));
                contactCommand.ExecuteNonQuery();
            }
            if (vehicles.Any())
            {
                using var vehicleCommand = _db.CreateCommand();
                vehicleCommand.CommandText = @"UPDATE vehicles
                    SET status = 'Inactive',
                    updated_at = @UpdatedAt
                    WHERE account_id = @Id";
                vehicleCommand.Parameters.Add(new SqliteParameter("@Id", id));
                vehicleCommand.Parameters.Add(new SqliteParameter("@UpdatedAt", DateTime.UtcNow));
                vehicleCommand.ExecuteNonQuery();
            }
            using var command = _db.CreateCommand();
            command.CommandText = @"UPDATE parking_accounts
                    SET status = 'Inactive',
                    updated_at = @UpdatedAt
                    WHERE id = @Id";

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

    public IEnumerable<Contact> GetContacts(int Id)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = "SELECT * FROM contacts WHERE status = 'Active' AND account_id = @AccountId";

        try
        {
            command.Parameters.Add(new SqliteParameter("@AccountId", Id));

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

    public IEnumerable<Vehicle> GetVehicles(int Id)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = "SELECT * FROM vehicles WHERE status = 'Active' AND account_id = @AccountId";

        try
        {
            command.Parameters.Add(new SqliteParameter("@AccountId", Id));

            using var reader = command.ExecuteReader();
            var vehicles = new List<Vehicle>();

            while (reader.Read())
            {
                vehicles.Add(new Vehicle
                {
                    Id = reader.GetInt32(0),
                    AccountId = reader.GetInt32(1),
                    Make = reader.GetString(2),
                    Model = reader.GetString(3),
                    Year = reader.GetInt32(4),
                    LicensePlate = reader.GetString(5),
                    Color = reader.GetString(6),
                    Type = reader.GetString(7),
                    Status = reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9),
                    UpdatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10)
                });
            }

            return vehicles;
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