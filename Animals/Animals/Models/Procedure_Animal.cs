using System.ComponentModel.DataAnnotations;

namespace Animals.Models;

public class Procedure_Animal
{
    [Required]
    public int AsocProcId { get; set; }
    [Required]
    public int AsocAnimalId { get; set; }
    [Required]
    public DateOnly Date { get; set; }
}