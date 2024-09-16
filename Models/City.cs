using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

[Table("City")]  
public class City : BaseModel
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; }

    [Required, StringLength(2)]
    public string StateUF { get; set; }

    [ForeignKey(nameof(StateUF))]
    public State State { get; set; }

    public ICollection<Client> Clients { get; set; } = new HashSet<Client>();
}
