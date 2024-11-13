using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Controllers.Operator;
using ITBees.Interfaces.Repository;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentServiceInfo
{
    PaginatedResult<PaymentVm> Get(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder);
    PaginatedResult<PaymentLogVm> GetLogs(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder);
}