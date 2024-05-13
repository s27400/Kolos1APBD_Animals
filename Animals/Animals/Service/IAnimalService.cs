using Animals.Models;

namespace Animals.Service;

public interface IAnimalService
{
    Task<bool> DoesAnimalExist(int id);
    Task<DTOGetAnimal> GetResult(int id);
    Task<string> PostVerification(DTOToAdd Animal);
}