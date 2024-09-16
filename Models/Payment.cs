using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyProject.Enums;

namespace MyProject.Models;

[Table("Payment")]
public class Payment : BaseModel
{
    [Key]
    public Guid Id { get; set; } 

    public DateTime? DataPayment { get; set; }

    [Required]
    public decimal Value { get; set; }

    [Required]
    public PaymentTypeEnum PaymentType { get; set; }

    [Required]
    public ReceivementTypeEnum ReceivementType { get; set; } 

    [Required]
    public Guid FkInstallmentId { get; set; } 

    [ForeignKey(nameof(FkInstallmentId))]
    public Installment Installment { get; set; }

    [Required]
    public Guid FkUserId { get; set; }

    [ForeignKey(nameof(FkUserId))]
    public User User { get; set; }
}
