using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

[Authorize]
public class AppleInAppPurchaseController : RestfulControllerBase<AppleInAppPurchaseController>
{
    private readonly IAppleInAppPurchaseService _appleInAppPurchaseService;

    public AppleInAppPurchaseController(ILogger<AppleInAppPurchaseController> logger, IAppleInAppPurchaseService appleInAppPurchaseService) : base(logger)
    {
        _appleInAppPurchaseService = appleInAppPurchaseService;
    }

    [HttpPost]
    [Produces<ApplePurchaseConfirmationVm>]
    public IActionResult Post([FromBody] ApplePurchaseIm applePurchaseIm)
    {
        return ReturnOkResult(() => _appleInAppPurchaseService.ConfirmPayment(applePurchaseIm));
    }
}