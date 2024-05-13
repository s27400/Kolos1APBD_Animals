using System.Transactions;
using Animals.Models;
using Animals.Repositories;
using Animals.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;

namespace Animals.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AnimalController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAnimal(int id)
    {
        if (!await _animalService.DoesAnimalExist(id))
        {
            return NotFound("Animal with this id not exist");
        }
         return Ok(await _animalService.GetResult(id));
    }

    [HttpPost]
    public async Task<IActionResult> AddNewAnimalWithProcedure(DTOToAdd Animal)
    {
        string response = await _animalService.PostVerification(Animal);
        if (response.StartsWith("Added"))
        {
            return Ok(response);
        }

        return NotFound(response);
    }
}