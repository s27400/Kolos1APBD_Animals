using System.ComponentModel.DataAnnotations;

namespace Animals.Models;

public class Animal
{
    [Required]
    public int IdAnimal { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Type { get; set; }
    [Required]
    public DateTime AdmissionDate { get; set; }
    [Required]
    public int Animal_IdOwner { get; set; }
    
}