using System.Transactions;
using Animals.Models;
using Animals.Repositories;

namespace Animals.Service;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;

    public AnimalService(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }
    
    public async Task<bool> DoesAnimalExist(int id)
    {
        return await _animalRepository.DoesAnimalExist(id);
    }

    public async Task<DTOGetAnimal> GetResult(int id)
    {
        return await _animalRepository.GetResult(id);
    }

    public async Task<string> PostVerification(DTOToAdd Animal)
    {
        if (!await _animalRepository.DoesOwnerExist(Animal.ownerId))
        {
            return $"Owner with {Animal.ownerId} not exist";
        }

        foreach (ProcedureDTO dto in Animal.procedures)
        {
            if (!await _animalRepository.DoesProcedureExist(dto.procedureId))
            {
                return $"Procedure with {dto.procedureId} not exist";
            }
        }
        try
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var res = await _animalRepository.AddAnimal(Animal);

                foreach (ProcedureDTO dto in Animal.procedures)
                {
                    await _animalRepository.AddProcedure(dto.procedureId, res, dto.date);
                }

                scope.Complete();
                return $"Added Animal id: {res}";
            }
        }
        catch (Exception e)
        {
            return "Error in transaction";
        }

    }
}