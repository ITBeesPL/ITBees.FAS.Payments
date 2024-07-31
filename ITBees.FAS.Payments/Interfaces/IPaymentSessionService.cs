using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentSessionService
{
    InitialisedPaymentLinkVm CreateNewPaymentSession(NewPaymentIm newPaymentIm);
}