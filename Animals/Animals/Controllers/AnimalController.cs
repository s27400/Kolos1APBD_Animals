using System.Transactions;
using Animals.Models;
using Animals.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;

namespace Animals.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AnimalController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;

    public AnimalController(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAnimal(int id)
    {
        if (!await _animalRepository.DoesAnimalExist(id))
        {
            return NotFound("Animal with this id not exist");
        }
         return Ok(await _animalRepository.GetResult(id));
    }

    [HttpPost]
    public async Task<IActionResult> AddNewAnimalWithProcedure(DTOToAdd Animal)
    {
        if (!await _animalRepository.DoesOwnerExist(Animal.ownerId))
        {
            return NotFound($"Owner with {Animal.ownerId} not exist");
        }

        foreach (ProcedureDTO dto in Animal.procedures)
        {
            if (!await _animalRepository.DoesProcedureExist(dto.procedureId))
            {
                return NotFound($"Procedure with {dto.procedureId} not exist");
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
                return Ok($"Added Animal id: {res}");
            }
        }
        catch (Exception e)
        {
            return NotFound("Error in transaction");
        }
    }
}