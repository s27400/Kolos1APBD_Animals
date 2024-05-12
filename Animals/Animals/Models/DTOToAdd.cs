using System.ComponentModel.DataAnnotations;

namespace Animals.Models;

public class DTOToAdd
{
    [Required]
    [MaxLength(100)]
    public string name { get; set; }
    [Required]
    [MaxLength(100)]
    public string type { get; set; }
    [Required]
    public DateTime admissionDate { get; set; }
    [Required]
    public int ownerId { get; set; }
    public IEnumerable<ProcedureDTO> procedures { get; set; }
}