using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentDbLoggerService
{
    void Log(PaymentOperatorLog log);
}