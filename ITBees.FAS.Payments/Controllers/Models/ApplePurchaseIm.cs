namespace ITBees.FAS.Payments.Controllers.Models;

public class ApplePurchaseIm
{
    public string ProductId { get; set; }
    public string? PaymentId { get; set; }
    public Guid PaymentSessionId { get; set; }
    public string? SessionData { get; set; }
}