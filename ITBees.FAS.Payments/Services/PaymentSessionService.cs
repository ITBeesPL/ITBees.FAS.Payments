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

    public PaymentSessionService(IWriteOnlyRepository<PaymentSession> paymentSessionRwRepo,
        IAspCurrentUserService aspCurrentUserService,
        IFasPaymentProcessor paymentProcessor)
    {
        _paymentSessionRwRepo = paymentSessionRwRepo;
        _aspCurrentUserService = aspCurrentUserService;
        _paymentProcessor = paymentProcessor;
    }


    public InitialisedPaymentLinkVm CreateNewPaymentSession(NewPaymentIm newPaymentIm)
    {
        var newPaymentSession = new PaymentSession()
        {
            Created = DateTime.Now,
            CreatedByGuid = _aspCurrentUserService.GetCurrentUserGuid(),
            Success = false,
            Finished = false,
            PaymentOperator = _paymentProcessor.GetType().Name
        };

        var paymentSession = _paymentSessionRwRepo.InsertData(newPaymentSession);

        var sessionUrl = _paymentProcessor.CreatePaymentSession(new FasPayment()
        {
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
}