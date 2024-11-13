using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Controllers.Models;

public class PaymentLogVm
{
    public PaymentLogVm()
    {
        
    }

    public PaymentLogVm(PaymentOperatorLog x)
    {
        Id = x.Id;
        Received = x.Received;
        Event = x.Event;
        Operator = x.Operator;
        JsonEvent = x.JsonEvent;
    }

    public int Id { get; set; }
    public DateTime Received { get; set; }
    public string Event { get; set; }
    public string Operator { get; set; }
    public string JsonEvent { get; set; }
}