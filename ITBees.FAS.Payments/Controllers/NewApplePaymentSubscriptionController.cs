using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

[Authorize]
public class NewApplePaymentSubscriptionController : RestfulControllerBase<NewApplePaymentSubscriptionController>
{
    private readonly IPaymentSubscriptionService _paymentSubscriptionService;

    public NewApplePaymentSubscriptionController(ILogger<NewApplePaymentSubscriptionController> logger,
        IPaymentSubscriptionService paymentSubscriptionService) : base(logger)
    {
        _paymentSubscriptionService = paymentSubscriptionService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newPaymentIm"></param>
    /// <returns></returns>
    [Produces(typeof(InitialisedApplePaymentVm))]
    [HttpPost]
    public IActionResult Post([FromBody] NewApplePaymentSubscriptionIm newApplePaymentSubscriptionIm)
    {
        return ReturnOkResult(() => _paymentSubscriptionService.CreateNewApplePaymentSubscriptionSession(newApplePaymentSubscriptionIm));
    }
}