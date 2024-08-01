using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

[Authorize]
public class NewPaymentSubscriptionController : RestfulControllerBase<NewPaymentSubscriptionController>
{
    private readonly IPaymentSessionService _paymentSessionService;
    private readonly IFasPaymentProcessor _paymentProcessor;

    public NewPaymentSubscriptionController(ILogger<NewPaymentSubscriptionController> logger,
        IPaymentSessionService paymentSessionService,
        IFasPaymentProcessor paymentProcessor) : base(logger)
    {
        _paymentSessionService = paymentSessionService;
        _paymentProcessor = paymentProcessor;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newPaymentIm"></param>
    /// <returns></returns>
    [Produces(typeof(InitialisedPaymentLinkVm))]
    [HttpPost]
    public IActionResult Post([FromBody] NewPaymentSubscriptionIm newPaymentSubscriptionIm)
    {

        InitialisedPaymentLinkVm result = _paymentSessionService.CreateNewPaymentSubscriptionSession(newPaymentSubscriptionIm);

        return Ok(result);
    }
}