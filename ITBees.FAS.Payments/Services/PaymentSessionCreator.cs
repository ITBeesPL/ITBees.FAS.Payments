using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;

namespace ITBees.FAS.Payments.Services;

class PaymentSessionCreator : IPaymentSessionCreator
{
    private readonly IWriteOnlyRepository<PaymentSession> _paymentSessionRwRepo;

    public PaymentSessionCreator(IWriteOnlyRepository<PaymentSession> paymentSessionRwRepo)
    {
        _paymentSessionRwRepo = paymentSessionRwRepo;
    }
    public PaymentSession CreateNew(DateTime Created, Guid? currentUserGuid,
        IFasPaymentProcessor paymentProcessor)
    {
        var newPaymentSession = new PaymentSession()
        {
            Created = DateTime.Now,
            CreatedByGuid = currentUserGuid,
            Success = false,
            Finished = false,
            PaymentOperator = paymentProcessor.ProcessorName
        };

        var paymentSession = _paymentSessionRwRepo.InsertData(newPaymentSession);
        return paymentSession;
    }
}