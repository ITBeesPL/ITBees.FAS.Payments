using ITBees.Models.Users;

namespace ITBees.FAS.Payments.Interfaces.Models;

public class PaymentSession
{
    public Guid Guid { get; set; }
    public UserAccount CreatedBy { get; set; }
    public Guid? CreatedByGuid { get; set; }
    public DateTime Created { get; set; }
    public bool Finished { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string PaymentOperator { get; set; }
}