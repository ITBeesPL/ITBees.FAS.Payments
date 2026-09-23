using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.Models.Companies;
using ITBees.Models.Payments;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Services;

class PaymentSessionCreator : IPaymentSessionCreator
{
    private readonly IWriteOnlyRepository<PaymentSession> _paymentSessionRwRepo;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;
    private readonly IApplySubscriptionPlanToCompanyService _applySubscriptionPlanToCompanyService;
    private readonly ILogger<PaymentSessionCreator> _logger;
    private readonly IOrderPackFinalizerService _orderPackFinalizerService;
    private readonly IReadOnlyRepository<PlatformSubscriptionPlan> _platformSubscriptionPlanRoRepo;
    private readonly IInvoiceDataService _invoiceDataService;
    private readonly ISuccessfulPaymentInvoiceIssuer _successfulPaymentInvoiceIssuer;

    public PaymentSessionCreator(
        IWriteOnlyRepository<PaymentSession> paymentSessionRwRepo,
        IReadOnlyRepository<PaymentSession> paymentSessionRoRepo,
        IApplySubscriptionPlanToCompanyService applySubscriptionPlanToCompanyService,
        ILogger<PaymentSessionCreator> logger,
        IOrderPackFinalizerService orderPackFinalizerService,
        IReadOnlyRepository<PlatformSubscriptionPlan> platformSubscriptionPlanRoRepo,
        IInvoiceDataService invoiceDataService,
        ISuccessfulPaymentInvoiceIssuer successfulPaymentInvoiceIssuer = null)
    {
        _paymentSessionRwRepo = paymentSessionRwRepo;
        _paymentSessionRoRepo = paymentSessionRoRepo;
        _applySubscriptionPlanToCompanyService = applySubscriptionPlanToCompanyService;
        _logger = logger;
        _orderPackFinalizerService = orderPackFinalizerService;
        _platformSubscriptionPlanRoRepo = platformSubscriptionPlanRoRepo;
        _invoiceDataService = invoiceDataService;
        _successfulPaymentInvoiceIssuer = successfulPaymentInvoiceIssuer;
    }

    public PaymentSession CreateNew(DateTime Created, Guid? currentUserGuid,
        IFasPaymentProcessor paymentProcessor, Guid invoiceDataGuid, string paymentOperator, Guid? orderPackGuid = null)
    {
        var newGuid = Guid.NewGuid();
        var newPaymentSession = new PaymentSession()
        {
            Created = DateTime.Now,
            CreatedByGuid = currentUserGuid,
            Success = false,
            Finished = false,
            PaymentOperator = string.IsNullOrEmpty(paymentOperator) ? paymentProcessor.ProcessorName : paymentOperator,
            InvoiceDataGuid = invoiceDataGuid,
            OrderPackGuid = orderPackGuid,
            FromSubscriptionRenew = false,
            Guid = newGuid,
            PaymentEventId = newGuid.ToString(), // beacuse we have unique index on (PaymentOperator, PaymentEventId) so its ineeded until we get real event id from payment operator
        };

        var paymentSession = _paymentSessionRwRepo.InsertData(newPaymentSession);
        return paymentSession;
    }

    public PaymentSession CreatePaymentSessionFromSubscriptionRenew(DateTime created, Guid? currentUserGuid,
        IFasPaymentProcessor paymentProcessor, Guid invoiceDataGuid, string paymentOperator, string paymentEventId,
        Guid? orderPackGuid = null,
        string? customerSubscriptionId = null, bool invoiceCreated = false)
    {
        var newPaymentSession = new PaymentSession()
        {
            Created = created,
            CreatedByGuid = currentUserGuid,
            Success = true,
            Finished = true,
            PaymentOperator = string.IsNullOrEmpty(paymentOperator) ? paymentProcessor.ProcessorName : paymentOperator,
            InvoiceDataGuid = invoiceDataGuid,
            OrderPackGuid = orderPackGuid,
            FromSubscriptionRenew = true,
            OperatorTransactionId = customerSubscriptionId,
            InvoiceCreated = invoiceCreated,
            FinishedDate = created,
            PaymentEventId = paymentEventId
        };
        PaymentSession? paymentSession = null;
        try
        {
            paymentSession = _paymentSessionRwRepo.InsertData(newPaymentSession);
        }
        catch (Exception e)
        {
            if (e.InnerException.Message.Contains("Duplicate"))
            {
                paymentSession = _paymentSessionRoRepo
                    .GetData(x => x.PaymentOperator == paymentOperator && x.PaymentEventId == paymentEventId)
                    .FirstOrDefault();
                if (paymentSession.FinishedDate == null)
                {
                    _paymentSessionRwRepo.UpdateData(x => x.Guid == paymentSession.Guid,
                        x => { x.FinishedDate = created; });
                }
            }
        }

        if (paymentSession != null)
        {
            TryIssueInvoiceForPaidSession(paymentSession.Guid);
        }

        return paymentSession;
    }

    /// <summary>
    /// Invokes the optional ISuccessfulPaymentInvoiceIssuer hook for an already-closed session,
    /// reloading it with InvoiceData and SubscriptionPlan as the hook contract requires. Never throws.
    /// </summary>
    private void TryIssueInvoiceForPaidSession(Guid paymentSessionGuid)
    {
        if (_successfulPaymentInvoiceIssuer == null)
            return;

        try
        {
            var sessionForInvoice = _paymentSessionRoRepo.GetData(x => x.Guid == paymentSessionGuid,
                    x => x.InvoiceData, x => x.InvoiceData.SubscriptionPlan)
                .FirstOrDefault();
            if (sessionForInvoice == null)
                return;

            _successfulPaymentInvoiceIssuer.IssueInvoiceForPaidSession(sessionForInvoice);
        }
        catch (Exception e)
        {
            // Invoice issuing must never break payment closing; the issuer is expected to retry on its own.
            _logger.LogError(e,
                $"ISuccessfulPaymentInvoiceIssuer failed for payment session {paymentSessionGuid}");
        }
    }

    public Company? TryGetCompanyWithSubscriptionPlanFromPaymentSubscriptionId(string stripeSubscriptionId)
    {
        if (string.IsNullOrEmpty(stripeSubscriptionId))
            return null;

        var result = _paymentSessionRoRepo.GetData(x => x.OperatorTransactionId == stripeSubscriptionId,
                x => x.InvoiceData, x => x.InvoiceData.Company.CompanyPlatformSubscription.SubscriptionPlan)
            .FirstOrDefault();
        try
        {
            return result.InvoiceData.Company;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public void CloseSuccessfulPayment(Guid guid, DateTime sessionCreated, string customerSubscriptionId,
        string paymentEventId = null)
    {
        CloseSuccessfulPayment(guid, sessionCreated, customerSubscriptionId, paymentEventId, null);
    }

    public void CloseSuccessfulPayment(Guid guid, DateTime sessionCreated, string customerSubscriptionId,
        string paymentEventId, Guid? paidSubscriptionPlanGuid)
    {
        _logger.LogDebug($"Closing payment session id : {paymentEventId} started...");


        var paymentSession = _paymentSessionRoRepo.GetFirst(x => x.Guid == guid,
            x => x.InvoiceData,
            x => x.InvoiceData.SubscriptionPlan, x => x.OrderPack);
        if (paymentSession.PaymentEventId == paymentEventId)
        {
            _logger.LogInformation($"Payment session id : {paymentEventId} already closed");
            return;
        }

        _logger.LogDebug($"Update payment session - guid {guid}");

        _paymentSessionRwRepo.UpdateData(x => x.Guid == guid, x =>
        {
            x.Finished = true;
            x.Success = true;
            x.FinishedDate = sessionCreated;
            x.OperatorTransactionId = customerSubscriptionId;
            x.PaymentEventId = paymentEventId;
        });

        if (paymentSession.OrderPackGuid != null)
        {
            _orderPackFinalizerService.CloseSuccessfullyPayedOrderPack(paymentSession.OrderPackGuid.Value);
        }
        else
        {
            _logger.LogDebug("Closing payment session finished...");
            var paidSubscriptionPlan = GetPaidSubscriptionPlan(paymentSession, paidSubscriptionPlanGuid);
            FreezePaidInvoiceData(paymentSession, paidSubscriptionPlan);

            _logger.LogDebug("Apply subscription plan stared...");
            _applySubscriptionPlanToCompanyService.Apply(paidSubscriptionPlan,
                paymentSession.InvoiceData.CompanyGuid.Value, sessionCreated);
        }

        _logger.LogDebug("Apply subscription plan finished...");

        // Reloaded, so the issuer sees the closed session with its frozen invoice data.
        TryIssueInvoiceForPaidSession(paymentSession.Guid);
    }

    /// <summary>
    /// The plan recorded by the payment operator at checkout wins over the plan on the session's invoice
    /// data: that row is shared by every checkout of the company, so opening another checkout (even an
    /// abandoned one, e.g. in a second tab) used to switch the plan of a checkout that was already open.
    /// </summary>
    private PlatformSubscriptionPlan GetPaidSubscriptionPlan(PaymentSession paymentSession,
        Guid? paidSubscriptionPlanGuid)
    {
        var invoiceDataPlan = paymentSession.InvoiceData.SubscriptionPlan;
        if (paidSubscriptionPlanGuid == null || paidSubscriptionPlanGuid == invoiceDataPlan?.Guid)
            return invoiceDataPlan;

        var paidSubscriptionPlan = _platformSubscriptionPlanRoRepo
            .GetData(x => x.Guid == paidSubscriptionPlanGuid.Value).FirstOrDefault();
        if (paidSubscriptionPlan == null)
        {
            _logger.LogError(
                "Paid subscription plan {PlanGuid} of payment session {SessionGuid} not found, using invoice data plan {InvoiceDataPlan}",
                paidSubscriptionPlanGuid, paymentSession.Guid, invoiceDataPlan?.PlanName);
            return invoiceDataPlan;
        }

        _logger.LogWarning(
            "Payment session {SessionGuid} was paid for plan {PaidPlan}, but its invoice data points to {InvoiceDataPlan} - using the paid plan",
            paymentSession.Guid, paidSubscriptionPlan.PlanName, invoiceDataPlan?.PlanName);
        return paidSubscriptionPlan;
    }

    /// <summary>
    /// Re-points the paid session to its own inactive copy of the invoice data bound to the paid plan
    /// (renewals already get such copies). Never throws - payment closing must not fail because of it.
    /// </summary>
    private void FreezePaidInvoiceData(PaymentSession paymentSession, PlatformSubscriptionPlan paidSubscriptionPlan)
    {
        if (paymentSession.InvoiceDataGuid == null || paidSubscriptionPlan == null)
            return;

        try
        {
            var snapshot = _invoiceDataService.CreatePaidSessionSnapshot(paymentSession.InvoiceDataGuid.Value,
                paidSubscriptionPlan);
            _paymentSessionRwRepo.UpdateData(x => x.Guid == paymentSession.Guid,
                x => { x.InvoiceDataGuid = snapshot.Guid; });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not freeze invoice data of paid payment session {SessionGuid}",
                paymentSession.Guid);
        }
    }
}