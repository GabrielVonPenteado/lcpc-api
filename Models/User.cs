using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

[Table("User")]
public class User : BaseModel
{
    [Key]
    public Guid Id { get; set; } 

    [Required, StringLength(20)]
    public string Username { get; set; } 

    [Required, StringLength(200)]
    public string Password { get; set; }

    [Required, StringLength(100)]
    public string Email { get; set; }
}
