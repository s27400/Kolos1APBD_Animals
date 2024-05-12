using System.ComponentModel.DataAnnotations;

namespace Animals.Models;

public class DTOGetAnimal
{
    [Required]
    public int id { get; set; }
    [Required]
    [MaxLength(100)]
    public string name { get; set; }
    [Required]
    [MaxLength(100)]
    public string type { get; set; }
    [Required]
    public DateTime admissionDate { get; set; }
    [Required]
    public Owner owner { get; set; }
    [Required]
    public IEnumerable<Procedure> procedures { get; set; }
}