using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Repository;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers.Operator;

public class PaymentsLogsController : RestfulControllerBase<PaymentsLogsController>
{
    private readonly IPaymentServiceInfo _paymentServiceInfo;

    public PaymentsLogsController(ILogger<PaymentsLogsController> logger, IPaymentServiceInfo paymentServiceInfo) : base(logger)
    {
        _paymentServiceInfo = paymentServiceInfo;
    }

    [HttpGet]
    [Produces<PaginatedResult<PaymentLogVm>>]
    public IActionResult Get(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        return ReturnOkResult(() => _paymentServiceInfo.GetLogs(authKey, page, pageSize, sortColumn, sortOrder));
    }
}