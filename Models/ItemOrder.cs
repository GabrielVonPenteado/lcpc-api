using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

[Table("ItemOrder")] 
public class ItemOrder : BaseModel
{
    [Required]
    public Guid FkProductId { get; set; }

    [ForeignKey(nameof(FkProductId))]
    public Product Product { get; set; }

    [Required]
    public Guid FkOrderId { get; set; }

    [ForeignKey(nameof(FkOrderId))]
    public Order Order { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal ItemValue { get; set; }
}
