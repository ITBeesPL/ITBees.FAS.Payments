using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.Models.Companies;
using ITBees.Models.Users;
using ITBees.RestfulApiControllers.Exceptions;
using ITBees.UserManager.Interfaces.Services;

namespace ITBees.FAS.Payments.Services;

public class InvoiceDataService : IInvoiceDataService
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IReadOnlyRepository<InvoiceData> _invoiceDataRoRepo;
    private readonly IWriteOnlyRepository<InvoiceData> _invoiceDataRwRepo;
    private readonly IReadOnlyRepository<Company> _companyRoRepo;

    public InvoiceDataService(IAspCurrentUserService aspCurrentUserService,
        IReadOnlyRepository<InvoiceData> invoiceDataRoRepo,
        IWriteOnlyRepository<InvoiceData> invoiceDataRwRepo,
        IReadOnlyRepository<Company> companyRoRepo)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _invoiceDataRoRepo = invoiceDataRoRepo;
        _invoiceDataRwRepo = invoiceDataRwRepo;
        _companyRoRepo = companyRoRepo;
    }
    public InvoiceDataVm Create(InvoiceDataIm invoiceDataIm)
    {
        var currentInvoiceData = _invoiceDataRoRepo.GetData(x => x.CompanyGuid == invoiceDataIm.CompanyGuid, x => x.SubscriptionPlan, x => x.Company, x => x.CreatedBy).FirstOrDefault();
        var cu = _aspCurrentUserService.GetCurrentUser();
        try
        {
            if (currentInvoiceData == null)
            {
                var result = _invoiceDataRwRepo.InsertData(new InvoiceData()
                {
                    City = invoiceDataIm.City == null ? "" : invoiceDataIm.City,
                    CompanyGuid = invoiceDataIm.CompanyGuid,
                    Country = invoiceDataIm.Country == null ? "" : invoiceDataIm.Country,
                    CompanyName = invoiceDataIm.CompanyName == null ? "" : invoiceDataIm.CompanyName,
                    Created = DateTime.Now,
                    CreatedByGuid = cu.Guid,
                    InvoiceEmail = invoiceDataIm.InvoiceEmail == null ? "" : invoiceDataIm.InvoiceEmail,
                    NIP = invoiceDataIm.NIP == null ? "" : invoiceDataIm.NIP,
                    PostCode = invoiceDataIm.PostCode == null ? "" : invoiceDataIm.PostCode,
                    Street = invoiceDataIm.Street == null ? "" : invoiceDataIm.Street,
                    SubscriptionPlanGuid = invoiceDataIm.SubscriptionPlanGuid
                });

                _invoiceDataRoRepo.GetData(x => x.Guid == result.Guid, x => x.Company, x => x.CreatedBy,
                    x => x.SubscriptionPlan);
                return new InvoiceDataVm(result);
            }
            else
            {
                currentInvoiceData.City = invoiceDataIm.City == null ? "" : invoiceDataIm.City;
                currentInvoiceData.CompanyGuid = invoiceDataIm.CompanyGuid;
                currentInvoiceData.Country = invoiceDataIm.Country == null ? "" : invoiceDataIm.Country;
                currentInvoiceData.CompanyName = invoiceDataIm.CompanyName == null ? "" : invoiceDataIm.CompanyName;
                currentInvoiceData.ModifiedByGuid = cu.Guid;
                currentInvoiceData.InvoiceEmail = invoiceDataIm.InvoiceEmail == null ? "" : invoiceDataIm.InvoiceEmail;
                currentInvoiceData.NIP = invoiceDataIm.NIP == null ? "" : invoiceDataIm.NIP;
                currentInvoiceData.PostCode = invoiceDataIm.PostCode == null ? "" : invoiceDataIm.PostCode;
                currentInvoiceData.Street = invoiceDataIm.Street == null ? "" : invoiceDataIm.Street;
                currentInvoiceData.SubscriptionPlanGuid = invoiceDataIm.SubscriptionPlanGuid;
                var updated = this.UpdateInvoiceData(invoiceDataIm, currentInvoiceData.Guid);
                return updated;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public InvoiceDataVm UpdateInvoiceData(InvoiceDataIm invoiceData, Guid invoiceDataGuid)
    {
        if (_aspCurrentUserService.TryCanIDoForCompany(TypeOfOperation.Rw, invoiceData.CompanyGuid) == false)
        {
            throw new AccessViolationException("You don't have enough privileges to set this data");
        }

        var result = _invoiceDataRwRepo.UpdateData(x => x.Guid == invoiceDataGuid, x =>
        {
            x.CompanyGuid = invoiceData.CompanyGuid ;
            x.Country = invoiceData.Country == null ? "" : invoiceData.Country;
            x.NIP = invoiceData.NIP == null ? "" : invoiceData.NIP;
            x.City = invoiceData.City == null ? "" : invoiceData.City;
            x.CompanyName = invoiceData.CompanyName == null ? "" : invoiceData.CompanyName;
            x.Country = invoiceData.Country == null ? "" : invoiceData.Country;
            x.ModifiedByGuid = _aspCurrentUserService.GetCurrentUserGuid();
            x.ModifiedDate = DateTime.Now;
            x.InvoiceEmail = invoiceData.InvoiceEmail == null ? "" : invoiceData.InvoiceEmail;
            x.PostCode = invoiceData.PostCode == null ? "" : invoiceData.PostCode;
            x.Street = invoiceData.Street == null ? "" : invoiceData.Street;
            x.SubscriptionPlanGuid = invoiceData.SubscriptionPlanGuid;
        }, x => x.SubscriptionPlan, x => x.CreatedBy, x => x.Company);

        return new InvoiceDataVm(result.FirstOrDefault());
    }

    public InvoiceDataVm Get(Guid companyGuid)
    {
        if (_aspCurrentUserService.TryCanIDoForCompany(TypeOfOperation.Ro, companyGuid) == false)
        {
            throw new AccessViolationException("You don't have enough privileges to see this data");
        }

        var invoiceData = _invoiceDataRoRepo.GetFirst(x => x.CompanyGuid == companyGuid && x.IsActive,
            x => x.ModifiedBy, x => x.CreatedBy,
            x => x.Company,
            x => x.SubscriptionPlan);

        if (invoiceData == null)
        {
            var company = _companyRoRepo.GetData(x => x.Guid == companyGuid).FirstOrDefault();
            if (company == null)
            {
                throw new ResultNotFoundException("Company not found");
            }

            var entity = new InvoiceData()
            {
                City = company.City,
                CompanyGuid = company.Guid,
                CompanyName = company.CompanyName,
                Country = "",
                Created = DateTime.Now,
                CreatedByGuid = _aspCurrentUserService.GetCurrentUserGuid().Value,
                InvoiceEmail = _aspCurrentUserService.GetCurrentUser().Email,
                InvoiceRequested = string.IsNullOrEmpty(company.Nip) ? true : false,
                IsActive = true,
                NIP = company.Nip,
                PostCode = company.PostCode,
                Street = company.Street,
                SubscriptionPlanGuid = null
            };

            var result = _invoiceDataRwRepo.InsertData(entity);

            return new InvoiceDataVm(_invoiceDataRoRepo.GetFirst(x => x.Guid == result.Guid, x => x.Company,
                x => x.CreatedBy, x => x.SubscriptionPlan));
        }

        return new InvoiceDataVm(invoiceData);
    }

    public InvoiceDataVm Update(InvoiceDataUm invoiceDataUm)
    {
        var invoiceData = _invoiceDataRoRepo.GetData(x => x.Guid == invoiceDataUm.Guid).FirstOrDefault();
        return this.UpdateInvoiceData(invoiceDataUm, invoiceDataUm.Guid);
    }
}