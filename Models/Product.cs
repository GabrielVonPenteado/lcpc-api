using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyProject.Enums;

namespace MyProject.Models;

[Table("Product")]
public class Product : BaseModel
{
    [Key]
    public Guid Id { get; set; } 

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required]
    public ProductTypeEnum ProductType { get; set; }

    [StringLength(200)]
    public string Description { get; set; }

    [Required]
    public decimal Value { get; set; } 

    [Required]
    public decimal Thickness { get; set; }

    [Required]
    public decimal Width { get; set; }

    [Required]
    public decimal Length { get; set; }
}
