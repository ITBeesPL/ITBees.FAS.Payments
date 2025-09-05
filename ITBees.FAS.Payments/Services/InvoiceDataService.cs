using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.Models.Companies;
using ITBees.Models.Payments;
using ITBees.Models.Users;
using ITBees.RestfulApiControllers.Exceptions;
using ITBees.UserManager.Interfaces;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Services;

public class InvoiceDataService : IInvoiceDataService
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IReadOnlyRepository<InvoiceData> _invoiceDataRoRepo;
    private readonly IWriteOnlyRepository<InvoiceData> _invoiceDataRwRepo;
    private readonly IReadOnlyRepository<Company> _companyRoRepo;
    private readonly ILogger<InvoiceDataService> _logger;
    private readonly IWriteOnlyRepository<PaymentSession> _paymentSessionRwRepo;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;

    public InvoiceDataService(IAspCurrentUserService aspCurrentUserService,
        IReadOnlyRepository<InvoiceData> invoiceDataRoRepo,
        IWriteOnlyRepository<InvoiceData> invoiceDataRwRepo,
        IReadOnlyRepository<Company> companyRoRepo,
        ILogger<InvoiceDataService> logger,
        IWriteOnlyRepository<PaymentSession> paymentSessionRwRepo,
        IReadOnlyRepository<PaymentSession> paymentSessionRoRepo)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _invoiceDataRoRepo = invoiceDataRoRepo;
        _invoiceDataRwRepo = invoiceDataRwRepo;
        _companyRoRepo = companyRoRepo;
        _logger = logger;
        _paymentSessionRwRepo = paymentSessionRwRepo;
        _paymentSessionRoRepo = paymentSessionRoRepo;
    }

    public InvoiceDataVm Create(InvoiceDataIm invoiceDataIm, bool allowUpdateIfExists)
    {
        var currentInvoiceData = _invoiceDataRoRepo.GetData(x => x.CompanyGuid == invoiceDataIm.CompanyGuid,
            x => x.SubscriptionPlan, x => x.Company, x => x.CreatedBy).FirstOrDefault();
        var cu = _aspCurrentUserService.GetCurrentUser();
        try
        {
            bool createNew = currentInvoiceData == null || allowUpdateIfExists == false;

            Guid createdByGuid;

            if (cu != null && cu.Guid != Guid.Empty)
            {
                createdByGuid = cu.Guid;
            }
            else if (currentInvoiceData != null)
            {
                createdByGuid = currentInvoiceData.CreatedBy.Guid;
            }
            else
            {
                throw new InvalidOperationException(
                    "No current user and no existing invoice data to copy CreatedBy from.");
            }

            if (createNew)
            {
                var result = _invoiceDataRwRepo.InsertData(new InvoiceData()
                {
                    City = invoiceDataIm.City == null ? "" : invoiceDataIm.City,
                    CompanyGuid = invoiceDataIm.CompanyGuid,
                    Country = invoiceDataIm.Country == null ? "" : invoiceDataIm.Country,
                    CompanyName = invoiceDataIm.CompanyName == null ? "" : invoiceDataIm.CompanyName,
                    Created = DateTime.Now,
                    CreatedByGuid = createdByGuid,
                    InvoiceEmail = invoiceDataIm.InvoiceEmail == null ? "" : invoiceDataIm.InvoiceEmail,
                    NIP = invoiceDataIm.NIP == null ? "" : invoiceDataIm.NIP,
                    PostCode = invoiceDataIm.PostCode == null ? "" : invoiceDataIm.PostCode,
                    Street = invoiceDataIm.Street == null ? "" : invoiceDataIm.Street,
                    SubscriptionPlanGuid = invoiceDataIm.SubscriptionPlanGuid,
                    InvoiceRequested = invoiceDataIm.InvoiceRequested,
                    IsActive = invoiceDataIm.IsActive
                });

                result = _invoiceDataRoRepo.GetData(x => x.Guid == result.Guid, x => x.Company, x => x.CreatedBy,
                    x => x.SubscriptionPlan).FirstOrDefault();
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
            x.CompanyGuid = invoiceData.CompanyGuid;
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
            x.InvoiceRequested = invoiceData.InvoiceRequested;
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
            return CreateNewEmptyInvoiceData(companyGuid);
        }

        return new InvoiceDataVm(invoiceData);
    }

    public InvoiceDataVm CreateNewEmptyInvoiceData(Guid companyGuid)
    {
        var company = _companyRoRepo.GetData(x => x.Guid == companyGuid).FirstOrDefault();
        if (company == null)
        {
            throw new ResultNotFoundException("Company not found");
        }

        return CreateNewInvoiceData(company);
    }

    private InvoiceDataVm CreateNewInvoiceData(Company company, InvoiceData lastInvoiceData = null,
        PlatformSubscriptionPlan platformSubscriptionPlan = null)
    {
        InvoiceData entity = null;
        if (lastInvoiceData == null)
        {
            entity = new InvoiceData()
            {
                City = company.City == null ? "" : company.City,
                CompanyGuid = company.Guid,
                CompanyName = company.CompanyName,
                Country = "",
                Created = DateTime.Now,
                CreatedByGuid = _aspCurrentUserService.GetCurrentUserGuid().Value,
                InvoiceEmail = _aspCurrentUserService.GetCurrentUser().Email,
                InvoiceRequested = string.IsNullOrEmpty(company.Nip) ? false : true,
                IsActive = true,
                NIP = company.Nip == null ? "" : company.Nip,
                PostCode = company.PostCode == null ? "" : company.PostCode,
                Street = company.Street == null ? "" : company.Street,
                SubscriptionPlanGuid = platformSubscriptionPlan?.Guid
            };
        }
        else
        {
            var newInvoiceData = new InvoiceData()
            {
                City = lastInvoiceData.City == null ? "" : lastInvoiceData.City,
                CompanyGuid = company.Guid,
                Country = lastInvoiceData.Country == null ? "" : lastInvoiceData.Country,
                CompanyName = lastInvoiceData.CompanyName == null ? "" : lastInvoiceData.CompanyName,
                InvoiceEmail = lastInvoiceData.InvoiceEmail == null ? "" : lastInvoiceData.InvoiceEmail,
                NIP = lastInvoiceData.NIP == null ? "" : lastInvoiceData.NIP,
                PostCode = lastInvoiceData.PostCode == null ? "" : lastInvoiceData.PostCode,
                Street = lastInvoiceData.Street == null ? "" : lastInvoiceData.Street,
                SubscriptionPlanGuid = platformSubscriptionPlan?.Guid,
                InvoiceRequested = lastInvoiceData.InvoiceRequested,
                IsActive = true
            };
        }

        var result = _invoiceDataRwRepo.InsertData(entity);

        return new InvoiceDataVm(_invoiceDataRoRepo.GetFirst(x => x.Guid == result.Guid, x => x.Company,
            x => x.CreatedBy, x => x.SubscriptionPlan));
    }

    public InvoiceDataVm CreateNewInvoiceBasedOnLastInvoice(Company company,
        PlatformSubscriptionPlan platformSubscriptionPlan)
    {
        var existingInvoiceData = _invoiceDataRoRepo.GetData(x => x.CompanyGuid == company.Guid)
            .OrderByDescending(x => x.Created).FirstOrDefault();
        if (existingInvoiceData == null)
        {
            var message =
                $"No invoice data found for company : {company.CompanyName} (guid:{company.Guid}), cannot create new invoice data.";
            _logger.LogError(message);
            throw new Exception(message);
        }

        return CreateNewInvoiceData(company, existingInvoiceData, platformSubscriptionPlan);
    }

    public void CreateCorrectiveInvoiceForRefund(Guid companyGuid, decimal refundAmount, string subscriptionId)
    {
        var paymentSession = _paymentSessionRoRepo.GetData(x => x.OperatorTransactionId == subscriptionId)
            .FirstOrDefault();
        if (paymentSession == null)
        {
            var message = $"Payment session for {subscriptionId} not found, while trying to create corrective invoice.";
            _logger.LogError(message);
            throw new Exception(message);
        }

        //ToDo manager process of creating corrective invoice
        _paymentSessionRwRepo.UpdateData(x => x.Guid == paymentSession.Guid, x =>
        {
            x.Refunded = true;
            // x.CorrectiveInvoiceIssued = true //ToDo after creating corrective invoice
        });
    }

    public void CreateCorrectiveInvoiceForRefundForLastPaymentSession(Guid companyGuid)
    {
        var paymentSession = _paymentSessionRoRepo.GetData(x => x.InvoiceData.Company.Guid == companyGuid)
            .FirstOrDefault();
        CreateCorrectiveInvoiceForRefund(companyGuid, 0, paymentSession.OperatorTransactionId);
    }

    public InvoiceDataVm Update(InvoiceDataUm invoiceDataUm)
    {
        var invoiceData = _invoiceDataRoRepo.GetData(x => x.Guid == invoiceDataUm.Guid).FirstOrDefault();
        return this.UpdateInvoiceData(invoiceDataUm, invoiceDataUm.Guid);
    }
}