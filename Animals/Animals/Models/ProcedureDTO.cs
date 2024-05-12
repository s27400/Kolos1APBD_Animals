using System.ComponentModel.DataAnnotations;

namespace Animals.Models;

public class ProcedureDTO
{
    [Required]
    public int procedureId { get; set; }
    [Required]
    public DateTime date { get; set; }
    
}