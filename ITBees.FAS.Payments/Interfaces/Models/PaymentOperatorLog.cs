namespace ITBees.FAS.Payments.Interfaces.Models;

public class PaymentOperatorLog
{
    public int Id { get; set; }
    public DateTime Received { get; set; }
    public string Event { get; set; }
    public string Operator { get; set; }
    public string JsonEvent { get; set; }
}