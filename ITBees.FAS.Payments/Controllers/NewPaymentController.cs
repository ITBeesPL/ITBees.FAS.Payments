using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

public class NewPaymentController : RestfulControllerBase<NewPaymentController>
{
    private readonly IPaymentSessionService _paymentSessionService;
    private readonly IFasPaymentProcessor _paymentProcessor;

    public NewPaymentController(ILogger<NewPaymentController> logger, 
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
    public IActionResult Post([FromBody] NewPaymentIm newPaymentIm)
    {

        var result = _paymentSessionService.CreateNewPaymentSession(newPaymentIm);

        return Ok(result);
    }
}