using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.Models.Users;
using ITBees.UserManager.Interfaces.Services;

namespace ITBees.FAS.Payments.Services;

public class InvoiceDataService : IInvoiceDataService
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IReadOnlyRepository<InvoiceData> _invoiceDataRoRepo;
    private readonly IWriteOnlyRepository<InvoiceData> _invoiceDataRwRepo;

    public InvoiceDataService(IAspCurrentUserService aspCurrentUserService,
        IReadOnlyRepository<InvoiceData> invoiceDataRoRepo,
        IWriteOnlyRepository<InvoiceData> invoiceDataRwRepo)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _invoiceDataRoRepo = invoiceDataRoRepo;
        _invoiceDataRwRepo = invoiceDataRwRepo;
    }
    public InvoiceDataVm Create(InvoiceDataIm invoiceDataIm)
    {
        var currentInvoiceData = _invoiceDataRoRepo.GetData(x => x.CompanyGuid == invoiceDataIm.CompanyGuid,x=>x.SubscriptionPlan, x=>x.Company, x=>x.CreatedBy).FirstOrDefault();
        var cu = _aspCurrentUserService.GetCurrentUser();
        try
        {
            if (currentInvoiceData == null)
            {
                var result = _invoiceDataRwRepo.InsertData(new InvoiceData()
                {
                    City = invoiceDataIm.City,
                    CompanyGuid = invoiceDataIm.CompanyGuid,
                    Country = invoiceDataIm.Country,
                    CompanyName = invoiceDataIm.CompanyName,
                    Created = DateTime.Now,
                    CreatedByGuid = cu.Guid,
                    InvoiceEmail = invoiceDataIm.InvoiceEmail,
                    NIP = invoiceDataIm.NIP,
                    PostCode = invoiceDataIm.PostCode,
                    Street = invoiceDataIm.Street,
                    SubscriptionPlanGuid = invoiceDataIm.SubscriptionPlanGuid
                });

                _invoiceDataRoRepo.GetData(x => x.Guid == result.Guid, x => x.Company, x => x.CreatedBy,
                    x => x.SubscriptionPlan);
                return new InvoiceDataVm(result);
            }
            else
            {
                currentInvoiceData.City = invoiceDataIm.City;
                currentInvoiceData.CompanyGuid = invoiceDataIm.CompanyGuid;
                currentInvoiceData.Country = invoiceDataIm.Country;
                currentInvoiceData.CompanyName = invoiceDataIm.CompanyName;
                currentInvoiceData.ModifiedByGuid = cu.Guid;
                currentInvoiceData.InvoiceEmail = invoiceDataIm.InvoiceEmail;
                currentInvoiceData.NIP = invoiceDataIm.NIP;
                currentInvoiceData.PostCode = invoiceDataIm.PostCode;
                currentInvoiceData.Street = invoiceDataIm.Street;
                currentInvoiceData.SubscriptionPlanGuid = invoiceDataIm.SubscriptionPlanGuid;
                var updated= this.UpdateInvoiceData(currentInvoiceData);
                return updated;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public InvoiceDataVm UpdateInvoiceData(InvoiceData invoiceData)
    {
        if (_aspCurrentUserService.TryCanIDoForCompany(TypeOfOperation.Rw, invoiceData.CompanyGuid) == false)
        {
            throw new AccessViolationException("You don't have enough privileges to set this data");
        }

        var result = _invoiceDataRwRepo.UpdateData(x => x.Guid == invoiceData.Guid, x =>
        {
            x.CompanyGuid = invoiceData.CompanyGuid;
            x.Country = invoiceData.Country;
            x.NIP = invoiceData.NIP;
            x.City = invoiceData.City;
            x.CompanyName = invoiceData.CompanyName;
            x.Country = invoiceData.Country;
            x.ModifiedByGuid = _aspCurrentUserService.GetCurrentUserGuid();
            x.ModifiedDate = DateTime.Now;
            x.InvoiceEmail = invoiceData.InvoiceEmail;
            x.PostCode = invoiceData.PostCode;
            x.Street = invoiceData.Street;
            x.SubscriptionPlanGuid = invoiceData.SubscriptionPlanGuid;
        }, x=>x.SubscriptionPlan, x=>x.CreatedBy, x=>x.Company);

        return new InvoiceDataVm(result.FirstOrDefault());
    }

    public InvoiceDataVm Get(Guid companyGuid)
    {
        if (_aspCurrentUserService.TryCanIDoForCompany(TypeOfOperation.Ro, companyGuid) == false)
        {
            throw new AccessViolationException("You don't have enough privileges to see this data");
        }

        var invoiceData = _invoiceDataRoRepo.GetFirst(x => x.CompanyGuid == companyGuid, 
            x => x.ModifiedBy, x => x.CreatedBy,
            x => x.Company, 
            x=>x.SubscriptionPlan);

        return new InvoiceDataVm(invoiceData);
    }

    public InvoiceDataVm Update(InvoiceDataUm invoiceDataUm)
    {
        var invoiceData = _invoiceDataRoRepo.GetData(x => x.Guid == invoiceDataUm.Guid).FirstOrDefault();
        return this.UpdateInvoiceData(invoiceData);
    }
}