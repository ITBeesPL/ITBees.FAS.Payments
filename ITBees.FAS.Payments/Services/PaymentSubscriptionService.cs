using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Platforms;
using ITBees.Interfaces.Repository;
using ITBees.Models.Companies;
using ITBees.Models.Payments;
using ITBees.RestfulApiControllers.Exceptions;
using ITBees.RestfulApiControllers.Models;
using ITBees.UserManager.Interfaces;

namespace ITBees.FAS.Payments.Services;

class PaymentSubscriptionService : IPaymentSubscriptionService
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IReadOnlyRepository<PlatformSubscriptionPlan> _platformSubscriptionPlanRoRepo;
    private readonly IReadOnlyRepository<InvoiceData> _invoiceDataRoRepo;
    private readonly IWriteOnlyRepository<InvoiceData> _invoiceDataRwRepo;
    private readonly IFasPaymentProcessor _paymentProcessor;
    private readonly IPaymentSessionCreator _paymentSessionCreator;
    private readonly IApplySubscriptionPlanAsPlatformOperatorService _applySubscriptionPlanAsPlatformOperatorService;
    private readonly IPlatformSettingsService _platformSettingsService;
    private readonly IInvoiceDataService _invoiceDataService;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;

    public PaymentSubscriptionService(IAspCurrentUserService aspCurrentUserService,
        IReadOnlyRepository<PlatformSubscriptionPlan> platformSubscriptionPlanRoRepo,
        IReadOnlyRepository<InvoiceData> invoiceDataRoRepo,
        IWriteOnlyRepository<InvoiceData> invoiceDataRwRepo,
        IFasPaymentProcessor paymentProcessor,
        IPaymentSessionCreator paymentSessionCreator,
        IApplySubscriptionPlanAsPlatformOperatorService applySubscriptionPlanAsPlatformOperatorService,
        IPlatformSettingsService platformSettingsService,
        IInvoiceDataService invoiceDataService)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _platformSubscriptionPlanRoRepo = platformSubscriptionPlanRoRepo;
        _invoiceDataRoRepo = invoiceDataRoRepo;
        _invoiceDataRwRepo = invoiceDataRwRepo;
        _paymentProcessor = paymentProcessor;
        _paymentSessionCreator = paymentSessionCreator;
        _applySubscriptionPlanAsPlatformOperatorService = applySubscriptionPlanAsPlatformOperatorService;
        _platformSettingsService = platformSettingsService;
        _invoiceDataService = invoiceDataService;
    }

    public InitialisedPaymentLinkVm CreateNewPaymentSubscriptionSession(
        NewPaymentSubscriptionIm newPaymentSubscriptionIm, string paymentOperator, DateTime startingFrom)
    {
        var invoiceData = _invoiceDataRoRepo
            .GetData(x => x.CompanyGuid == newPaymentSubscriptionIm.CompanyGuid && x.IsActive).FirstOrDefault();

        if (invoiceData == null)
        {
            var newEmptyInvoiceData = _invoiceDataService.CreateNewEmptyInvoiceData(newPaymentSubscriptionIm.CompanyGuid);
            invoiceData = new InvoiceData()
            {
                Guid = newEmptyInvoiceData.Guid,
                CompanyGuid = newEmptyInvoiceData.CompanyGuid,
            };
        }
        if (invoiceData == null)
        {
            throw new Exception("Could not find user invoice data");
        }

        _invoiceDataRwRepo.UpdateData(x => x.Guid == newPaymentSubscriptionIm.InvoiceDataGuid, x =>
        {
            x.SubscriptionPlanGuid = newPaymentSubscriptionIm.PlatformSubscriptionPlanGuid;
        });

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
                SubscriptionPlanGuid = subcriptionPlan.Guid,
                StartingFrom = startingFrom
            });

            return new InitialisedPaymentLinkVm(_platformSettingsService.GetSetting("PlatformRedirectUrlAfterTrialPlanEnabled"), null);
        }

        var paymentSession = _paymentSessionCreator.CreateNew(DateTime.Now, _aspCurrentUserService.GetCurrentUserGuid(),
            _paymentProcessor, newPaymentSubscriptionIm.InvoiceDataGuid, paymentOperator);

        var fasBillingPeriod = BillingPeriod.GetBillingPeriod(subcriptionPlan.Interval);
        var fasPayment = new FasPayment()
        {
            Mode = subcriptionPlan.IsOneTimePayment ? FasPaymentMode.Payment : FasPaymentMode.Subscription,
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
        var result = _paymentProcessor.CreatePaymentSession(fasPayment, subcriptionPlan.IsOneTimePayment, newPaymentSubscriptionIm.SuccessUrl, newPaymentSubscriptionIm.FailureUrl);

        return new InitialisedPaymentLinkVm(result.SessionUrl, paymentSession.Guid);
    }

    public InitialisedApplePaymentVm CreateNewApplePaymentSubscriptionSession(
        NewApplePaymentSubscriptionIm newApplePaymentSubscriptionIm)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        var invoiceData = _invoiceDataService.CreateNewEmptyInvoiceData(cu.LastUsedCompanyGuid);
        var subscriptionPlan = _platformSubscriptionPlanRoRepo.GetData(x => x.AppleProductId == newApplePaymentSubscriptionIm.AppleProductId).FirstOrDefault();

        if (subscriptionPlan == null)
            throw new FasApiErrorException(new FasApiErrorVm(
                $"Could not find subscription plan for provided apple product Id : {newApplePaymentSubscriptionIm.AppleProductId}", 404, ""));

        var paymentSession = this.CreateNewPaymentSubscriptionSession(new NewPaymentSubscriptionIm()
        {
            CompanyGuid = cu.LastUsedCompanyGuid,
            FailureUrl = string.Empty,
            SuccessUrl = string.Empty,
            InvoiceDataGuid = invoiceData.Guid,
            PlatformSubscriptionPlanGuid = subscriptionPlan.Guid,
        }, "ApplePay", DateTime.Now);

        return new InitialisedApplePaymentVm(paymentSession);
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