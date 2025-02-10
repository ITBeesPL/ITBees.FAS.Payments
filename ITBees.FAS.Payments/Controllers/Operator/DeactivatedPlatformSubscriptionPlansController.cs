using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers.Operator;

[Authorize(Roles = Role.PlatformOperator)]
public class DeactivatedPlatformSubscriptionPlansController : RestfulControllerBase<DeactivatedPlatformSubscriptionPlansController>
{
    private readonly IPlatformAvailableSubscriptionPlansService _availableSubscriptionPlansService;

    public DeactivatedPlatformSubscriptionPlansController(ILogger<DeactivatedPlatformSubscriptionPlansController> logger, IPlatformAvailableSubscriptionPlansService availableSubscriptionPlansService) : base(logger)
    {
        _availableSubscriptionPlansService = availableSubscriptionPlansService;
    }

    [HttpGet]
    [Produces(typeof(List<PlatformSubscriptionPlanVm>))]
    public IActionResult Get([FromHeader(Name = "Accept-Language")] string acceptLanguage)
    {
        return ReturnOkResult(() => _availableSubscriptionPlansService.GetAllDectivatedPlans(acceptLanguage));
    }
}