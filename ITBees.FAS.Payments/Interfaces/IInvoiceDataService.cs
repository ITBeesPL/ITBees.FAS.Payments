using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IInvoiceDataService
{
    InvoiceDataVm Create(InvoiceDataIm invoiceDataIm, bool allowUpdateIfExist);
    InvoiceDataVm Get(Guid companyGuid);
    InvoiceDataVm Update(InvoiceDataUm invoiceDataUm);
    InvoiceDataVm CreateNewEmptyInvoiceData(Guid companyGuid);
}