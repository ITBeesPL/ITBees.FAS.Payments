using ITBees.FAS.Payments.Controllers.Models;

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
}