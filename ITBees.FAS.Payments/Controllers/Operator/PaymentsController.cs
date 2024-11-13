using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Repository;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers.Operator;

public class PaymentsController : RestfulControllerBase<PaymentsController>
{
    private readonly IPaymentServiceInfo _paymentServiceInfo;

    public PaymentsController(ILogger<PaymentsController> logger, IPaymentServiceInfo paymentServiceInfo) : base(logger)
    {
        _paymentServiceInfo = paymentServiceInfo;
    }

    [HttpGet]
    [Produces<PaginatedResult<PaymentVm>>]
    public IActionResult Get(string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        return ReturnOkResult(() => _paymentServiceInfo.Get(authKey, page, pageSize, sortColumn, sortOrder));
    }
}