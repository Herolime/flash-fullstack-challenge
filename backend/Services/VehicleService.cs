using Backend.Models;
using Backend.Models.SearchParams;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Backend.Services;

public class VehicleService : IVehicleService
{
    private readonly IDbConnection _db;

    public VehicleService(IDbConnection db)
    {
        _db = db;
    }

    public IEnumerable<Vehicle> Search(VehicleSearchParams searchParams)
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

        if (!string.IsNullOrEmpty(searchParams.Make))
        {
            conditions.Add("make LIKE @Make");
            command.Parameters.Add(new SqliteParameter("@Make", $"%{searchParams.Make}%"));
        }

        if (!string.IsNullOrEmpty(searchParams.Model))
        {
            conditions.Add("model LIKE @Model");
            command.Parameters.Add(new SqliteParameter("@Model", $"%{searchParams.Model}%"));
        }

        if (searchParams.Year.HasValue)
        {
            conditions.Add("year = @Year");
            command.Parameters.Add(new SqliteParameter("@Year", searchParams.Year.Value));
        }

        if (!string.IsNullOrEmpty(searchParams.LicensePlate))
        {
            conditions.Add("license_plate LIKE @LicensePlate");
            command.Parameters.Add(new SqliteParameter("@LicensePlate", $"%{searchParams.LicensePlate}%"));
        }

        if (!string.IsNullOrEmpty(searchParams.Color))
        {
            conditions.Add("color LIKE @Color");
            command.Parameters.Add(new SqliteParameter("@Color", $"%{searchParams.Color}%"));
        }

        if (!string.IsNullOrEmpty(searchParams.Type))
        {
            conditions.Add("type LIKE @Type");
            command.Parameters.Add(new { Type = $"%{searchParams.Type}%" });
        }

        if (!string.IsNullOrEmpty(searchParams.Status))
        {
            conditions.Add("status LIKE @Status");
            command.Parameters.Add(new { Status = $"%{searchParams.Status}%" });
        }

        var whereClause = conditions.Count > 0 ? $" WHERE {string.Join(" AND ", conditions)}" : string.Empty;
        command.CommandText = $"SELECT * FROM vehicles{whereClause}";

        try
        {
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
                    Color = reader.IsDBNull(6) ? null : reader.GetString(6),
                    Type = reader.IsDBNull(7) ? null : reader.GetString(7),
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

    public Vehicle? GetById(int id)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = "SELECT * FROM vehicles WHERE status = 'Active' AND id = @Id";

        try
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Vehicle
            {
                Id = reader.GetInt32(0),
                AccountId = reader.GetInt32(1),
                Make = reader.GetString(2),
                Model = reader.GetString(3),
                Year = reader.GetInt32(4),
                LicensePlate = reader.GetString(5),
                Color = reader.IsDBNull(6) ? null : reader.GetString(6),
                Type = reader.IsDBNull(7) ? null : reader.GetString(7),
                CreatedAt = reader.GetDateTime(9),
                UpdatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10)
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

    public Vehicle Create(Vehicle vehicle)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = @"INSERT INTO vehicles 
            (account_id, make, model, year, license_plate, color, type, status, created_at) 
            VALUES (@AccountId, @Make, @Model, @Year, @LicensePlate, @Color, @Type, @Status, @CreatedAt);
            SELECT last_insert_rowid();";

        try
        {
            command.Parameters.Add(new SqliteParameter("@AccountId", vehicle.AccountId));
            command.Parameters.Add(new SqliteParameter("@Make", vehicle.Make));
            command.Parameters.Add(new SqliteParameter("@Model", vehicle.Model));
            command.Parameters.Add(new SqliteParameter("@Year", vehicle.Year));
            command.Parameters.Add(new SqliteParameter("@LicensePlate", vehicle.LicensePlate));
            command.Parameters.Add(new SqliteParameter("@Color", (object?)vehicle.Color ?? DBNull.Value));
            command.Parameters.Add(new SqliteParameter("@Type", (object?)vehicle.Type ?? DBNull.Value));
            command.Parameters.Add(new SqliteParameter("@Status", "Active"));
            command.Parameters.Add(new SqliteParameter("@CreatedAt", DateTime.UtcNow));

            vehicle.Id = Convert.ToInt32(command.ExecuteScalar());
            vehicle.CreatedAt = DateTime.UtcNow;
            return vehicle;
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

    public void Update(int id, Vehicle vehicle)
    {
        _db.Open();
        using var command = _db.CreateCommand();
        command.CommandText = @"UPDATE vehicles 
            SET account_id = @AccountId, 
                make = @Make, 
                model = @Model, 
                year = @Year, 
                license_plate = @LicensePlate, 
                color = @Color, 
                type = @Type, 
                updated_at = @UpdatedAt 
            WHERE id = @Id";

        try
        {
            command.Parameters.Add(new SqliteParameter("@Id", id));
            command.Parameters.Add(new SqliteParameter("@AccountId", vehicle.AccountId));
            command.Parameters.Add(new SqliteParameter("@Make", vehicle.Make));
            command.Parameters.Add(new SqliteParameter("@Model", vehicle.Model));
            command.Parameters.Add(new SqliteParameter("@Year", vehicle.Year));
            command.Parameters.Add(new SqliteParameter("@LicensePlate", vehicle.LicensePlate));
            command.Parameters.Add(new SqliteParameter("@Color", (object?)vehicle.Color ?? DBNull.Value));
            command.Parameters.Add(new SqliteParameter("@Type", (object?)vehicle.Type ?? DBNull.Value));
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
        command.CommandText = @"UPDATE vehicles 
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