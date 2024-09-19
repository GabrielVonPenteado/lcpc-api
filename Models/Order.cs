using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyProject.Enums;

namespace MyProject.Models;

[Table("Order")] 
public class Order : BaseModel
{
    [Key]
    public Guid Id { get; set; } 

    [StringLength(512)]
    public string Description { get; set; } 

    [Required]
    public decimal TotalValue { get; set; }

    public DateTime? ShippingDate { get; set; }

    [Required]
    public DateTime CreationDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    [Required]
    public DateTime ExpectedDeliveryDate { get; set; }

    [Required]
    public OrderStatus Status { get; set; }

    [Required]
    public int NInstallments { get; set; }

    [Required]
    public Guid FkUserId { get; set; }

    [ForeignKey(nameof(FkUserId))]
    public User User { get; set; }

    [Required]
    public Guid FkClientId { get; set; }

    [ForeignKey(nameof(FkClientId))]
    public Client Client { get; set; }

    public ICollection<Installment> Installments { get; set; } = new HashSet<Installment>();
    public ICollection<ItemOrder> ItensOrder { get; set; } = new HashSet<ItemOrder>();
}
