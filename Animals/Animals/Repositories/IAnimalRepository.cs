using Animals.Models;

namespace Animals.Repositories;

public interface IAnimalRepository
{
    Task<bool> DoesAnimalExist(int id);
    
    Task<Animal> GetAnimalById(int id);
    Task<Owner> GetOwner(int ownerId);
    Task<IEnumerable<Procedure>> GetAllProcedures(int id);
    Task<DTOGetAnimal> GetResult(int id);

    Task<bool> DoesOwnerExist(int id);
    Task<bool> DoesProcedureExist(int id);

    Task<int> AddAnimal(DTOToAdd Animal);
    public Task<int> AddProcedure(int procedureId, int animalId, DateTime date);

}