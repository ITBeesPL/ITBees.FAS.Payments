using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

public class PaymentCanceledController : RestfulControllerBase<PaymentCanceledController>
{
    private readonly IPaymentSessionService _paymentSessionService;

    public PaymentCanceledController(ILogger<PaymentCanceledController> logger, 
        IPaymentSessionService paymentSessionService) : base(logger)
    {
        _paymentSessionService = paymentSessionService;
    }

    [HttpGet]
    public IActionResult Get(Guid paymentSessionGuid)
    {
        return ReturnOkResult(() => _paymentSessionService.CancelPayment(paymentSessionGuid));
    }
}