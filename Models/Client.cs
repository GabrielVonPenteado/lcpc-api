using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

[Table("Client")] 
public class Client : BaseModel
{
    [Key]
    public Guid Id { get; set; } 

    [Required, StringLength(50)]
    public string Name { get; set; }

    [Required, StringLength(100)]
    public string Streetplace { get; set; }

    [Required, StringLength(100)]
    public string Neighborhood { get; set; }

    [Required, StringLength(6)] 
    public string Number { get; set; } 

    [StringLength(255)]
    public string Complement { get; set; }

    [Required, StringLength(15)] 
    public string Phone { get; set; }

    [Required, StringLength(100)]
    public string Email { get; set; }

    [Required, StringLength(14)] 
    public string CNPJ { get; set; }

    [Required]
    public Guid FkCityId { get; set; }

    [ForeignKey(nameof(FkCityId))]
    public City City { get; set; }  

    public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
}
