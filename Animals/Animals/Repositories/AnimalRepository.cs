using System.Data.SqlClient;
using Animals.Models;

namespace Animals.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;

    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesAnimalExist(int id)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Animal WHERE ID = @AnimalId";
        cmd.Parameters.AddWithValue("@AnimalId", id);
        if (await cmd.ExecuteScalarAsync() is null)
        {
            return false;
        }

        return true;
    }


    public async Task<Animal> GetAnimalById(int id)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT ID, Name, Type, AdmissionDate, Owner_ID FROM Animal WHERE ID = @id";
        cmd.Parameters.AddWithValue("@id", id);

        var reader =  await cmd.ExecuteReaderAsync();
        Animal animal = new Animal();

        while (await reader.ReadAsync())
        {
            animal = new Animal()
            {
                IdAnimal = (int)reader["ID"],
                Name = reader["Name"].ToString(),
                Type = reader["Type"].ToString(),
                AdmissionDate = (DateTime)reader["AdmissionDate"],
                Animal_IdOwner = (int)reader["Owner_ID"]
            };
        }
        return animal;
    }

    public async Task<Owner> GetOwner(int ownerId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT ID, FirstName, LastName FROM Owner WHERE ID = @id";
        cmd.Parameters.AddWithValue("@id", ownerId);
        
        var reader = await cmd.ExecuteReaderAsync();

        Owner owner = new Owner();

        while (await reader.ReadAsync())
        {
            owner = new Owner()
            {
                IdOwner = (int)reader["ID"],
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString()
            };
        }

        return owner;
    }

    public async Task<IEnumerable<Procedure>> GetAllProcedures(int id)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "SELECT Name, Description, Date FROM [Procedure] INNER JOIN [Procedure_Animal] ON [Procedure].ID = [Procedure_Animal].Procedure_ID WHERE Animal_ID = @id";
        cmd.Parameters.AddWithValue("@id", id);

        var reader = await cmd.ExecuteReaderAsync();
        List<Procedure> allProcedures = new List<Procedure>();

        while (await reader.ReadAsync())
        {
            allProcedures.Add(new Procedure()
            {
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                date = (DateTime)reader["Date"]
            });
        }

        return allProcedures;
    }

    public async Task<DTOGetAnimal> GetResult(int id)
    {
        Animal a = await GetAnimalById(id);

        Owner o = await GetOwner(a.Animal_IdOwner);

        IEnumerable<Procedure> list = await GetAllProcedures(id);

        DTOGetAnimal res = new DTOGetAnimal()
        {
            id = a.IdAnimal,
            name = a.Name,
            type = a.Type,
            admissionDate = a.AdmissionDate,
            owner = o,
            procedures = list
        };
        return res;
    }
    

    public async Task<bool> DoesOwnerExist(int id)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Owner WHERE ID = @id";
        cmd.Parameters.AddWithValue("@id", id);
        if (await cmd.ExecuteScalarAsync() is null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DoesProcedureExist(int id)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM [Procedure] WHERE ID = @id";
        cmd.Parameters.AddWithValue("@id", id);
        if (await cmd.ExecuteScalarAsync() is null)
        {
            return false;
        }

        return true;
    }

    public async Task<int> AddAnimal(DTOToAdd Animal)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO Animal(Name, Type, AdmissionDate, Owner_ID) VALUES (@name, @type, @date, @oId); SELECT @@IDENTITY AS ID;";
        cmd.Parameters.AddWithValue("@name", Animal.name);
        cmd.Parameters.AddWithValue("@type", Animal.type);
        cmd.Parameters.AddWithValue("@date", Animal.admissionDate);
        cmd.Parameters.AddWithValue("@oId", Animal.ownerId);

        var res = await cmd.ExecuteScalarAsync();
        if (res is null)
        {
            return -99;
        }

        return Convert.ToInt32(res);
    }

    public async Task AddProcedure(ProcedureDTO procedureDto, int AnimalId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO Procedure_Animal Values(@pID, @aID, @date)";
        cmd.Parameters.AddWithValue("@pID", procedureDto.procedureId);
        cmd.Parameters.AddWithValue("@aID", AnimalId);
        cmd.Parameters.AddWithValue("@date", DateOnly.FromDateTime(procedureDto.date));

        await cmd.ExecuteNonQueryAsync();
    }
    
    
}