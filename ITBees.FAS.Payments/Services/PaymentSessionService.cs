using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.UserManager.Interfaces.Services;

namespace ITBees.FAS.Payments.Services;

public class PaymentSessionService : IPaymentSessionService
{
    private readonly IWriteOnlyRepository<PaymentSession> _paymentSessionRwRepo;
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IFasPaymentProcessor _paymentProcessor;
    private readonly IPaymentSessionCreator _paymentSessionCreator;

    public PaymentSessionService(IWriteOnlyRepository<PaymentSession> paymentSessionRwRepo,
        IAspCurrentUserService aspCurrentUserService,
        IFasPaymentProcessor paymentProcessor,
        IPaymentSessionCreator paymentSessionCreator)
    {
        _paymentSessionRwRepo = paymentSessionRwRepo;
        _aspCurrentUserService = aspCurrentUserService;
        _paymentProcessor = paymentProcessor;
        _paymentSessionCreator = paymentSessionCreator;
    }


    public InitialisedPaymentLinkVm CreateNewPaymentSession(NewPaymentIm newPaymentIm)
    {
        var currentUserGuid = _aspCurrentUserService.GetCurrentUserGuid();
        PaymentSession paymentSession = _paymentSessionCreator.CreateNew(Created: DateTime.Now, currentUserGuid, _paymentProcessor);

        var sessionUrl = _paymentProcessor.CreatePaymentSession(new FasPayment()
        {
            PaymentSessionGuid = paymentSession.Guid,
            Mode = FasPaymentMode.Subscription,
            Products = new List<FasProduct>()
         {
             new FasProduct()
             {
                 BillingPeriod = FasBillingPeriod.Monthly,
                 Currency = newPaymentIm.Currency,
                 Quantity = newPaymentIm.Quantity,
                 PaymentTitleOrProductName = newPaymentIm.ProductName,
                 Price = newPaymentIm.Price,
                 Interval = newPaymentIm.Interval,
                 IntervalCount = newPaymentIm.IntervalCount
             }
         }
        });

        return new InitialisedPaymentLinkVm(sessionUrl.SessionUrl);
    }

    public bool ConfirmPayment(Guid paymentSessionGuid)
    {
        return _paymentProcessor.ConfirmPayment(paymentSessionGuid);
    }

    public void CancelPayment(Guid paymentSessionGuid)
    {
        _paymentSessionRwRepo.UpdateData(x => x.Guid == paymentSessionGuid && x.Finished == false, x =>
        {
            x.Finished = true;
            x.Success = false;
        });
    }
}