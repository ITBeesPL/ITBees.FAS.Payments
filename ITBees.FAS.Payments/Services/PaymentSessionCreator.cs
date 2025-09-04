using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.Models.Companies;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Services;

class PaymentSessionCreator : IPaymentSessionCreator
{
    private readonly IWriteOnlyRepository<PaymentSession> _paymentSessionRwRepo;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;
    private readonly IApplySubscriptionPlanToCompanyService _applySubscriptionPlanToCompanyService;
    private readonly ILogger<PaymentSessionCreator> _logger;
    private readonly IOrderPackFinalizerService _orderPackFinalizerService;

    public PaymentSessionCreator(
        IWriteOnlyRepository<PaymentSession> paymentSessionRwRepo,
        IReadOnlyRepository<PaymentSession> paymentSessionRoRepo,
        IApplySubscriptionPlanToCompanyService applySubscriptionPlanToCompanyService,
        ILogger<PaymentSessionCreator> logger,
        IOrderPackFinalizerService orderPackFinalizerService)
    {
        _paymentSessionRwRepo = paymentSessionRwRepo;
        _paymentSessionRoRepo = paymentSessionRoRepo;
        _applySubscriptionPlanToCompanyService = applySubscriptionPlanToCompanyService;
        _logger = logger;
        _orderPackFinalizerService = orderPackFinalizerService;
    }

    public PaymentSession CreateNew(DateTime Created, Guid? currentUserGuid,
        IFasPaymentProcessor paymentProcessor, Guid invoiceDataGuid, string paymentOperator, Guid? orderPackGuid = null)
    {
        var newPaymentSession = new PaymentSession()
        {
            Created = DateTime.Now,
            CreatedByGuid = currentUserGuid,
            Success = false,
            Finished = false,
            PaymentOperator = string.IsNullOrEmpty(paymentOperator) ? paymentProcessor.ProcessorName : paymentOperator,
            InvoiceDataGuid = invoiceDataGuid,
            OrderPackGuid = orderPackGuid,
            FromSubscriptionRenew = false
        };

        var paymentSession = _paymentSessionRwRepo.InsertData(newPaymentSession);
        return paymentSession;
    }

    public PaymentSession CreatePaymentSessionFromSubscriptionRenew(DateTime Created, Guid? currentUserGuid,
        IFasPaymentProcessor paymentProcessor, Guid invoiceDataGuid, string paymentOperator, Guid? orderPackGuid = null,
        string? customerSubscriptionId = null, bool invoiceCreated = false)
    {
        var newPaymentSession = new PaymentSession()
        {
            Created = DateTime.Now,
            CreatedByGuid = currentUserGuid,
            Success = false,
            Finished = false,
            PaymentOperator = string.IsNullOrEmpty(paymentOperator) ? paymentProcessor.ProcessorName : paymentOperator,
            InvoiceDataGuid = invoiceDataGuid,
            OrderPackGuid = orderPackGuid,
            FromSubscriptionRenew = true,
            OperatorTransactionId = customerSubscriptionId,
            InvoiceCreated = invoiceCreated
        };

        var paymentSession = _paymentSessionRwRepo.InsertData(newPaymentSession);
        return paymentSession;
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

    public void CloseSuccessfulPayment(Guid guid, string customerSubscriptionId = null)
    {
        _logger.LogDebug("Closing payment session started...");


        var paymentSession = _paymentSessionRoRepo.GetFirst(x => x.Guid == guid,
            x => x.InvoiceData,
            x => x.InvoiceData.SubscriptionPlan, x => x.OrderPack);

        _logger.LogDebug($"Update payment session - guid {guid}");

        _paymentSessionRwRepo.UpdateData(x => x.Guid == guid, x =>
        {
            x.Finished = true;
            x.Success = true;
            x.FinishedDate = DateTime.Now;
            x.OperatorTransactionId = customerSubscriptionId;
        });

        if (paymentSession.OrderPackGuid != null)
        {
            _orderPackFinalizerService.CloseSuccessfullyPayedOrderPack(paymentSession.OrderPackGuid.Value);
        }
        else
        {
            _logger.LogDebug("Closing payment session finished...");
            //to do service responsible for managing existing platform subscription on maybe active
            _logger.LogDebug("Apply subscription plan stared...");
            _applySubscriptionPlanToCompanyService.Apply(paymentSession.InvoiceData.SubscriptionPlan,
                paymentSession.InvoiceData.CompanyGuid);
        }

        _logger.LogDebug("Apply subscription plan finished...");
    }
}