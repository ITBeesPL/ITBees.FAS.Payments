using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IInvoiceDataService
{ 
    // allowUpdateIfExist = true - ensures that if an invoice data record already exists for the given company, it will be updated with the new data.
    // allowUpdateIfExist = false - ensures that if an invoice data record already exists for the given company, a new record will be created instead of updating the existing one.
    InvoiceDataVm Create(InvoiceDataIm invoiceDataIm, bool allowUpdateIfExist = true);
    InvoiceDataVm Get(Guid companyGuid);
    InvoiceDataVm Update(InvoiceDataUm invoiceDataUm);
    InvoiceDataVm CreateNewEmptyInvoiceData(Guid companyGuid);
}