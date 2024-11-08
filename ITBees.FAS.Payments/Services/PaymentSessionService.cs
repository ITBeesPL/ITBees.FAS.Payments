using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.RestfulApiControllers.Exceptions;
using ITBees.UserManager.Interfaces;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Services;

public class PaymentSessionService : IPaymentSessionService
{
    private readonly IWriteOnlyRepository<PaymentSession> _paymentSessionRwRepo;
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IFasPaymentProcessor _paymentProcessor;
    private readonly IPaymentSessionCreator _paymentSessionCreator;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;
    private readonly IApplySubscriptionPlanToCompanyService _applySubscriptionPlanToCompanyService;
    private readonly ILogger<PaymentSessionService> _logger;

    public PaymentSessionService(IWriteOnlyRepository<PaymentSession> paymentSessionRwRepo,
        IAspCurrentUserService aspCurrentUserService,
        IFasPaymentProcessor paymentProcessor,
        IPaymentSessionCreator paymentSessionCreator,
        IReadOnlyRepository<PaymentSession> paymentSessionRoRepo,
        IApplySubscriptionPlanToCompanyService applySubscriptionPlanToCompanyService,
        ILogger<PaymentSessionService> logger)
    {
        _paymentSessionRwRepo = paymentSessionRwRepo;
        _aspCurrentUserService = aspCurrentUserService;
        _paymentProcessor = paymentProcessor;
        _paymentSessionCreator = paymentSessionCreator;
        _paymentSessionRoRepo = paymentSessionRoRepo;
        _applySubscriptionPlanToCompanyService = applySubscriptionPlanToCompanyService;
        _logger = logger;
    }


    public InitialisedPaymentLinkVm CreateNewPaymentSession(NewPaymentIm newPaymentIm)
    {
        var currentUserGuid = _aspCurrentUserService.GetCurrentUserGuid();
        PaymentSession paymentSession = _paymentSessionCreator.CreateNew(Created: DateTime.Now, currentUserGuid, _paymentProcessor, newPaymentIm.InvoiceDataGuid, string.Empty);

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
        }, true, newPaymentIm.SuccessUrl, newPaymentIm.FailureUrl);

        return new InitialisedPaymentLinkVm(sessionUrl.SessionUrl, paymentSession.Guid);
    }

    public bool ConfirmPayment(Guid paymentSessionGuid)
    {
        var paymentSession = _paymentSessionRoRepo.GetData(x => x.Guid == paymentSessionGuid, x => x.InvoiceData, x => x.InvoiceData.SubscriptionPlan).FirstOrDefault();
        if (paymentSession == null)
            throw new ResultNotFoundException("PaymentSession not exist");
        if (paymentSession.Finished)
        {
            return true;
        }

        var paymentFinishedWithSuccessOnStripe = _paymentProcessor.ConfirmPayment(paymentSessionGuid);
        if (paymentFinishedWithSuccessOnStripe)
        {
            _paymentSessionRwRepo.UpdateData(x => x.Guid == paymentSessionGuid, x =>
            {
                x.Finished = true;
                x.FinishedDate = new DateTime();
                x.Success = true;
            });

            _applySubscriptionPlanToCompanyService.Apply(paymentSession.InvoiceData.SubscriptionPlan, paymentSession.InvoiceData.CompanyGuid);
        }

        return paymentFinishedWithSuccessOnStripe;
    }

    public void CancelPayment(Guid paymentSessionGuid)
    {
        _paymentSessionRwRepo.UpdateData(x => x.Guid == paymentSessionGuid && x.Finished == false, x =>
        {
            x.Finished = true;
            x.Success = false;
        });
    }

    public bool UserCompanyHasEverMadePayment(Guid userAccountGuid)
    {
        var paymentSession = _paymentSessionRoRepo.GetData(x => x.InvoiceData.Company.OwnerGuid == userAccountGuid).FirstOrDefault();
        if (paymentSession != null)
        {
            return true;
        }

        return false;
    }

    public bool FakeAppleConfirmPayment(Guid paymentSessionId)
    {
        try
        {
            var paymentSession = _paymentSessionRoRepo.GetData(x => x.Guid == paymentSessionId,
                    x => x.InvoiceData,
                    x => x.InvoiceData.SubscriptionPlan)
                .FirstOrDefault();

            if (paymentSession == null)
                throw new ResultNotFoundException("PaymentSession not exist");

            if (paymentSession.PaymentOperator != "ApplePay")
                throw new UnauthorizedAccessException();

            _paymentSessionRwRepo.UpdateData(x => x.Guid == paymentSessionId, x =>
            {
                x.Finished = true;
                x.FinishedDate = new DateTime();
                x.Success = true;
            });

            _applySubscriptionPlanToCompanyService.Apply(paymentSession.InvoiceData.SubscriptionPlan, paymentSession.InvoiceData.CompanyGuid);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, $"Error for payment session id :{paymentSessionId}");
            return false;
        }
    }
}