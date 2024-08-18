﻿using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;

namespace ITBees.FAS.Payments.Services;

class PaymentSessionCreator : IPaymentSessionCreator
{
    private readonly IWriteOnlyRepository<PaymentSession> _paymentSessionRwRepo;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;
    private readonly IApplySubscriptionPlanToCompanyService _applySubscriptionPlanToCompanyService;

    public PaymentSessionCreator(
        IWriteOnlyRepository<PaymentSession> paymentSessionRwRepo, 
        IReadOnlyRepository<PaymentSession> paymentSessionRoRepo, 
        IApplySubscriptionPlanToCompanyService applySubscriptionPlanToCompanyService)
    {
        _paymentSessionRwRepo = paymentSessionRwRepo;
        _paymentSessionRoRepo = paymentSessionRoRepo;
        _applySubscriptionPlanToCompanyService = applySubscriptionPlanToCompanyService;
    }
    public PaymentSession CreateNew(DateTime Created, Guid? currentUserGuid,
        IFasPaymentProcessor paymentProcessor, Guid invoiceDataGuid)
    {
        var newPaymentSession = new PaymentSession()
        {
            Created = DateTime.Now,
            CreatedByGuid = currentUserGuid,
            Success = false,
            Finished = false,
            PaymentOperator = paymentProcessor.ProcessorName,
            InvoiceDataGuid = invoiceDataGuid
        };

        var paymentSession = _paymentSessionRwRepo.InsertData(newPaymentSession);
        return paymentSession;
    }

    public void CloseSuccessfulPayment(Guid guid)
    {
        var pa = _paymentSessionRoRepo.GetData(x => x.Guid == guid);
        var paymentSession = _paymentSessionRoRepo.GetFirst(x => x.Guid == guid, 
            x=>x.InvoiceData, 
            x=>x.InvoiceData.SubscriptionPlan);
        _paymentSessionRwRepo.UpdateData(x => x.Guid == guid, x =>
        {
            x.Finished = true;
            x.Success = true;
            x.FinishedDate = DateTime.Now;
        });

        //to do service responsible for managing existing platform subscription on maybe actvie
        _applySubscriptionPlanToCompanyService.Apply(paymentSession.InvoiceData.SubscriptionPlan, paymentSession.InvoiceData.CompanyGuid);

    }
}