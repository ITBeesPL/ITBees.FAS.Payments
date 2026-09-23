using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Models.Companies;
using ITBees.Models.Payments;

namespace ITBees.FAS.Payments.Interfaces;

public interface IInvoiceDataService
{ 
    /// <param name="allowUpdateIfExist">
    /// Defines the behavior when an invoice record for the company already exists:
    /// - true  → update the existing record with the new data
    /// - false → create a new record instead of updating the existing one
    /// </param>
    InvoiceDataVm Create(InvoiceDataIm invoiceDataIm, bool allowUpdateIfExist = true);
    InvoiceDataVm Get(Guid companyGuid);
    InvoiceDataVm Update(InvoiceDataUm invoiceDataUm);
    InvoiceDataVm CreateNewEmptyInvoiceData(Guid companyGuid);
    InvoiceDataVm CreateNewInvoiceBasedOnLastInvoice(Company companyGuid,
        PlatformSubscriptionPlan platformSubscriptionPlan);

    /// <summary>
    /// Creates an inactive copy of the given invoice data bound to the plan that was actually paid for.
    /// A paid payment session is re-pointed to this copy, so later edits of the company's shared invoice
    /// data (e.g. picking another plan in a new, maybe abandoned, checkout) can't change what the paid
    /// session - and the invoice issued for it - was for.
    /// </summary>
    InvoiceDataVm CreatePaidSessionSnapshot(Guid sourceInvoiceDataGuid,
        PlatformSubscriptionPlan platformSubscriptionPlan);
    void CreateCorrectiveInvoiceForRefund(Guid companyGuid, decimal refundAmount, string subscriptionId, PaymentSession? paymentSession = null);
    void CreateCorrectiveInvoiceForRefundForLastPaymentSession(Guid companyGuid);
}