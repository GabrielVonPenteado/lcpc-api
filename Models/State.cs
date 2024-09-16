using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

[Table("State")]
public class State
{
    [Key]
    [StringLength(2)]
    public string UF { get; set; } 

    [Required, StringLength(20)]
    public string Name { get; set; }
    public DateTime? DeletedAt { get; private set; }

    public ICollection<City> Cities { get; set; } = new HashSet<City>(); 
    public void SetDeletedAt()
    {
        DeletedAt = DateTime.UtcNow;
    }
    public void Restore()
    {
        DeletedAt = null;
    }
}
