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
    private readonly IApplySubscriptionPlanAsPlatformOperatorService _applySubscriptionPlanAsPlatformOperatorService;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;

    public PaymentSubscriptionService(IAspCurrentUserService aspCurrentUserService,
        IReadOnlyRepository<PlatformSubscriptionPlan> platformSubscriptionPlanRoRepo,
        IReadOnlyRepository<InvoiceData> invoiceDataRoRepo,
        IFasPaymentProcessor paymentProcessor,
        IPaymentSessionCreator paymentSessionCreator,
        IApplySubscriptionPlanAsPlatformOperatorService applySubscriptionPlanAsPlatformOperatorService)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _platformSubscriptionPlanRoRepo = platformSubscriptionPlanRoRepo;
        _invoiceDataRoRepo = invoiceDataRoRepo;
        _paymentProcessor = paymentProcessor;
        _paymentSessionCreator = paymentSessionCreator;
        _applySubscriptionPlanAsPlatformOperatorService = applySubscriptionPlanAsPlatformOperatorService;
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

        if (subcriptionPlan.IsTrial)
        {
            _applySubscriptionPlanAsPlatformOperatorService.Apply(new ApplySubscriptionPlanToCompanyIm()
            {
                CompanyGuid = invoiceData.CompanyGuid,
                SubscriptionPlanGuid = subcriptionPlan.Guid
            });
        }

        var paymentSession = _paymentSessionCreator.CreateNew(DateTime.Now, _aspCurrentUserService.GetCurrentUserGuid(),
            _paymentProcessor);

        var fasBillingPeriod = BillingPeriod.GetBillingPeriod(subcriptionPlan.Interval);
        var fasPayment = new FasPayment()
        {
            Mode = FasPaymentMode.Subscription,
            PaymentSessionGuid = paymentSession.Guid,
            Products = new List<FasProduct>(){new FasProduct()
            {
                Currency = subcriptionPlan.Currency,
                BillingPeriod = fasBillingPeriod,
                Quantity = 1,
                PaymentTitleOrProductName = subcriptionPlan.PlanName,
                Price = subcriptionPlan.Value,
                Interval = subcriptionPlan.Interval.ToString(),
                IntervalCount = GetMaximumIntervalCount(fasBillingPeriod)
            }},
            CustomerEmail = _aspCurrentUserService.GetCurrentUser().Email,
            CustomerName = _aspCurrentUserService.GetCurrentUser().DisplayName,
        };
        var result = _paymentProcessor.CreatePaymentSession(fasPayment);

        return new InitialisedPaymentLinkVm(result.SessionUrl);
    }

    private int GetMaximumIntervalCount(FasBillingPeriod fasBillingPeriod)
    {
        switch (fasBillingPeriod)
        {
            case FasBillingPeriod.Daily:
                return 1080;
                break;
            case FasBillingPeriod.Weekly:
                return 156;
                break;
            case FasBillingPeriod.Monthly:
                return 36;
                break;
            case FasBillingPeriod.Every3Months:
                return 12;
                break;
            case FasBillingPeriod.Every6Months:
                return 6;
                break;
            case FasBillingPeriod.Yearly:
                return 3;
                break;
            case FasBillingPeriod.Custom:
                throw new ArgumentException("Custom billing period requires specific interval count.");
            default:
                throw new ArgumentOutOfRangeException(nameof(fasBillingPeriod), fasBillingPeriod, null);
        }
    }
}