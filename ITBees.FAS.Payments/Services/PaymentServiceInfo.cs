using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Platforms;
using ITBees.Interfaces.Repository;
using ITBees.RestfulApiControllers.Exceptions;
using ITBees.RestfulApiControllers.Models;
using ITBees.UserManager.Interfaces;

namespace ITBees.FAS.Payments.Services;

public class PaymentServiceInfo : IPaymentServiceInfo
{
    private readonly IPlatformSettingsService _platformSettingsService;
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IReadOnlyRepository<PaymentOperatorLog> _paymentOperatorLogRoRepo;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;

    public PaymentServiceInfo(IPlatformSettingsService platformSettingsService,
        IAspCurrentUserService aspCurrentUserService,
        IReadOnlyRepository<PaymentOperatorLog> paymentOperatorLogRoRepo,
        IReadOnlyRepository<PaymentSession> paymentSessionRoRepo)
    {
        _platformSettingsService = platformSettingsService;
        _aspCurrentUserService = aspCurrentUserService;
        _paymentOperatorLogRoRepo = paymentOperatorLogRoRepo;
        _paymentSessionRoRepo = paymentSessionRoRepo;
    }

    public PaginatedResult<PaymentVm> Get(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        CheckAccess(authKey);
        var sortOptions = new SortOptions(page, pageSize, sortColumn, sortOrder);
        var paginatedResult = _paymentSessionRoRepo
            .GetDataPaginated(x => true, sortOptions, x => x.CreatedBy, x => x.InvoiceData, x => x.InvoiceData.SubscriptionPlan)
            .MapTo(x => new PaymentVm(x));

        return paginatedResult;
    }

    public PaginatedResult<PaymentLogVm> GetLogs(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        CheckAccess(authKey);
        var sortOptions = new SortOptions(page, pageSize, sortColumn, sortOrder);
        var paginatedResult = _paymentOperatorLogRoRepo
            .GetDataPaginated(x => true, sortOptions)
            .MapTo(x => new PaymentLogVm(x));

        return paginatedResult;
    }

    private void CheckAccess(string? authKey)
    {
        if (string.IsNullOrEmpty(authKey) && _aspCurrentUserService.CurrentUserIsPlatformOperator() == false)
        {
            throw new FasApiErrorException(new FasApiErrorVm("Unauthorized access attempt", 401, ""));
        }

        if (authKey == _platformSettingsService.GetSetting("platformAuthKey"))
            return;

        if (_aspCurrentUserService.CurrentUserIsPlatformOperator())
            return;

        throw new FasApiErrorException(new FasApiErrorVm("Unauthorized access attempt", 401, ""));
    }
}