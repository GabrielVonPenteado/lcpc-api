using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

[Table("Installment")]
public class Installment : BaseModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime ExpirationDate { get; set; }

    [Required]
    public decimal Value { get; set; }

    [Required]
    public bool Situation { get; set; }

    [Required]
    public Guid FkOrderId { get; set; }

    [ForeignKey(nameof(FkOrderId))]
    public Order Order { get; set; }

    public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
}
