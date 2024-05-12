using System.ComponentModel.DataAnnotations;

namespace Animals.Models;

public class Procedure
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Description { get; set; }
    [Required]
    public DateTime date { get; set; }
}