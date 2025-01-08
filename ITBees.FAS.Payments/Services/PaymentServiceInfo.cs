using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Controllers.Operator;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Platforms;
using ITBees.Interfaces.Repository;
using ITBees.UserManager.Interfaces;

namespace ITBees.FAS.Payments.Services;

public class PaymentServiceInfo : IPaymentServiceInfo
{
    private readonly IReadOnlyRepository<PaymentOperatorLog> _paymentOperatorLogRoRepo;
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRoRepo;
    private readonly IAccessChecker _accessChecker;

    public PaymentServiceInfo(
        IReadOnlyRepository<PaymentOperatorLog> paymentOperatorLogRoRepo,
        IReadOnlyRepository<PaymentSession> paymentSessionRoRepo, 
        IAccessChecker accessChecker)
    {
        _paymentOperatorLogRoRepo = paymentOperatorLogRoRepo;
        _paymentSessionRoRepo = paymentSessionRoRepo;
        _accessChecker = accessChecker;
    }

    public PaginatedResult<PaymentVm> Get(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        _accessChecker.CheckAccess(authKey);
        var sortOptions = new SortOptions(page, pageSize, sortColumn, sortOrder);
        var paginatedResult = _paymentSessionRoRepo
            .GetDataPaginated(x => true, sortOptions, x => x.CreatedBy, x => x.InvoiceData, x => x.InvoiceData.SubscriptionPlan)
            .MapTo(x => new PaymentVm(x));

        return paginatedResult;
    }

    public PaginatedResult<PaymentLogVm> GetLogs(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        _accessChecker.CheckAccess(authKey);
        var sortOptions = new SortOptions(page, pageSize, sortColumn, sortOrder);
        var paginatedResult = _paymentOperatorLogRoRepo
            .GetDataPaginated(x => true, sortOptions)
            .MapTo(x => new PaymentLogVm(x));

        return paginatedResult;
    }

    public PaginatedResult<FinishedPaymentVm> GetFinishedPayments(string? authKey, int? page, int? pageSize, string? sortColumn,
        SortOrder? sortOrder)
    {
        _accessChecker.CheckAccess(authKey);
        return _paymentSessionRoRepo.GetDataPaginated(x => x.Finished && x.Success,
            new SortOptions(page, pageSize, sortColumn, sortOrder),
            x => x.InvoiceData, x => x.InvoiceData.SubscriptionPlan,
            x => x.CreatedBy).MapTo(x => new FinishedPaymentVm(x));
    }
}