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

    [Produces<PaymnetSessionConfirmationVm>]
    public IActionResult Get(Guid paymentSessionGuid)
    {
        return ReturnOkResult(() => _paymentSessionService.ConfirmPayment(paymentSessionGuid));
    }
}

public class PaymnetSessionConfirmationVm
{
    public bool Success  { get; set; }
}