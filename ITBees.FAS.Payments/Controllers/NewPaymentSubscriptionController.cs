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
    private readonly IPaymentSubscriptionService _paymentSubscriptionService;

    public NewPaymentSubscriptionController(ILogger<NewPaymentSubscriptionController> logger,
        IPaymentSubscriptionService paymentSubscriptionService) : base(logger)
    {
        _paymentSubscriptionService = paymentSubscriptionService;
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
        return ReturnOkResult(() => _paymentSubscriptionService.CreateNewPaymentSubscriptionSession(newPaymentSubscriptionIm));
    }
}