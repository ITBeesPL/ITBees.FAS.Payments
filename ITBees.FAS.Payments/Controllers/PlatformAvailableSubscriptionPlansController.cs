using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

public class PlatformAvailableSubscriptionPlansController : RestfulControllerBase<PlatformAvailableSubscriptionPlansController>
{
    private readonly IPlatformAvailableSubscriptionPlansService _availableSubscriptionPlansService;

    public PlatformAvailableSubscriptionPlansController(ILogger<PlatformAvailableSubscriptionPlansController> logger, IPlatformAvailableSubscriptionPlansService availableSubscriptionPlansService) : base(logger)
    {
        _availableSubscriptionPlansService = availableSubscriptionPlansService;
    }

    [HttpGet]
    [Produces(typeof(List<PlatformSubscriptionPlanVm>))]
    public IActionResult Get([FromHeader(Name = "Accept-Language")] string acceptLanguage)
    {
        return ReturnOkResult(() => _availableSubscriptionPlansService.GetAllActivePlans(acceptLanguage));
    }
}