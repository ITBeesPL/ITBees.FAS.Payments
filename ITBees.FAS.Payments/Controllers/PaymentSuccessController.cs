using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

public class PaymentSuccessController : RestfulControllerBase<PaymentSuccessController>
{
    private readonly IPaymentSessionService _paymentSessionService;

    public PaymentSuccessController(ILogger<PaymentSuccessController> logger, IPaymentSessionService paymentSessionService) : base(logger)
    {
        _paymentSessionService = paymentSessionService;
    }

    [HttpGet]
    [Produces<PaymentSessionConfirmationVm>]
    public IActionResult Get(Guid paymentSessionGuid)
    {
        return ReturnOkResult(() => _paymentSessionService.ConfirmPayment(paymentSessionGuid));
    }
}