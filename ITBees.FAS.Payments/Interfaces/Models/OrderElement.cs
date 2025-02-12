using System.ComponentModel.DataAnnotations.Schema;

namespace ITBees.FAS.Payments.Interfaces.Models;

public class OrderElement
{
    public int Id { get; set; }
    public OrderPack OrderPack { get; set; }
    public Guid OrderPackGuid { get; set; }
    public Guid ProductGuid { get; set; }
    public int ItemsCount { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal UnitNetPrice { get; set; }
    public int VatPercentage { get; set; }
}