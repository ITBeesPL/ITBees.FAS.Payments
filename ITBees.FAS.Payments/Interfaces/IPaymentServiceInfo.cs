using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Controllers.Operator;
using ITBees.Interfaces.Repository;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPaymentServiceInfo
{
    PaginatedResult<PaymentVm> Get(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder);
    PaginatedResult<PaymentLogVm> GetLogs(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder);
    /// <summary>
    /// Retruns list of finished payments (successful and not refunded!)
    /// </summary>
    /// <param name="authKey"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <returns></returns>
    PaginatedResult<FinishedPaymentVm> GetFinishedPayments(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder);

    PaginatedResult<FinishedPaymentVm> GetFinishedAndRefundedPaymentsWithoutCorrectiveInvoiceIssued(string? authKey,
        int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder);
}