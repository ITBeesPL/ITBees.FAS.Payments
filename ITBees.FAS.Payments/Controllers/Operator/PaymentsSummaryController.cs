using ITBees.FAS.Payments.Services.Operator;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers.Operator;

public class PaymentsSummaryController : RestfulControllerBase<PaymentsSummaryController>
{
    private readonly IPaymentSummaryService _paymentSummaryService;

    public PaymentsSummaryController(ILogger<PaymentsSummaryController> logger, IPaymentSummaryService paymentSummaryService) : base(logger)
    {
        _paymentSummaryService = paymentSummaryService;
    }

    [HttpGet]
    [Produces<PaymentSummaryVm>]
    public IActionResult Get(bool viewAsHtml, string? authKey, int? month, int? year, int commisionPercentage, int vatPercentage)
    {
        return ReturnOkResult(()=>_paymentSummaryService.Get(authKey, month, year, commisionPercentage, vatPercentage));
    }
}