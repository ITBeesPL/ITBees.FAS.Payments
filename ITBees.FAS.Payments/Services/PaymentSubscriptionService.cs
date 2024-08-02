using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.RestfulApiControllers.Exceptions;
using ITBees.UserManager.Interfaces.Services;

namespace ITBees.FAS.Payments.Services;

class PaymentSubscriptionService : IPaymentSubscriptionService
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IReadOnlyRepository<PlatformSubscriptionPlan> _platformSubscriptionPlanRoRepo;
    private readonly IReadOnlyRepository<InvoiceData> _invoiceDataRoRepo;
    private readonly IFasPaymentProcessor _paymentProcessor;
    private readonly IPaymentSessionCreator _paymentSessionCreator;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;

    public PaymentSubscriptionService(IAspCurrentUserService aspCurrentUserService,
        IReadOnlyRepository<PlatformSubscriptionPlan> platformSubscriptionPlanRoRepo,
        IReadOnlyRepository<InvoiceData> invoiceDataRoRepo,
        IFasPaymentProcessor paymentProcessor,
        IPaymentSessionCreator paymentSessionCreator)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _platformSubscriptionPlanRoRepo = platformSubscriptionPlanRoRepo;
        _invoiceDataRoRepo = invoiceDataRoRepo;
        _paymentProcessor = paymentProcessor;
        _paymentSessionCreator = paymentSessionCreator;
    }

    public InitialisedPaymentLinkVm CreateNewPaymentSubscriptionSession(
        NewPaymentSubscriptionIm newPaymentSubscriptionIm)
    {
        var invoiceData = _invoiceDataRoRepo
            .GetData(x => x.CompanyGuid == newPaymentSubscriptionIm.CompanyGuid && x.IsActive).FirstOrDefault();
        if (invoiceData == null)
        {
            throw new Exception("Could not find user invoice data");
        }

        var subcriptionPlan = _platformSubscriptionPlanRoRepo
            .GetData(x => x.Guid == newPaymentSubscriptionIm.PlatformSubscriptionPlanGuid).FirstOrDefault();

        if (subcriptionPlan == null)
        {
            throw new ResultNotFoundException("Could not find subscription plan");
        }

        var paymentSession = _paymentSessionCreator.CreateNew(DateTime.Now, _aspCurrentUserService.GetCurrentUserGuid(),
            _paymentProcessor);

        var fasPayment = new FasPayment()
        {
            Mode = FasPaymentMode.Subscription,
            PaymentSessionGuid = paymentSession.Guid,
            Products = new List<FasProduct>(){new FasProduct()
            {
                Currency = subcriptionPlan.Currency,
                BillingPeriod = BillingPeriod.GetBillingPeriod(subcriptionPlan.Interval),
                Quantity = 1,
                PaymentTitleOrProductName = subcriptionPlan.PlanName,
                Price = subcriptionPlan.Value,
                Interval = subcriptionPlan.Interval.ToString(),
                IntervalCount = 36
            }},
            CustomerEmail = _aspCurrentUserService.GetCurrentUser().Email,
            CustomerName = _aspCurrentUserService.GetCurrentUser().DisplayName,
        };
        var result = _paymentProcessor.CreatePaymentSession(fasPayment);

        return new InitialisedPaymentLinkVm(result.SessionUrl);
    }
}