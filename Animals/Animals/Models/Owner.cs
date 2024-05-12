using System.ComponentModel.DataAnnotations;

namespace Animals.Models;

public class Owner
{
    [Required]
    public int IdOwner { get; set; }
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
}