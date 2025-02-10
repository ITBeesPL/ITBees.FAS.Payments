using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers.Operator;

[Authorize(Roles = Role.PlatformOperator)]
public class DeactivatedPlatformSubscriptionPlanController : RestfulControllerBase<DeactivatedPlatformSubscriptionPlanController>
{
    private readonly IPlatformAvailableSubscriptionPlansService _availableSubscriptionPlansService;

    public DeactivatedPlatformSubscriptionPlanController(ILogger<DeactivatedPlatformSubscriptionPlanController> logger, IPlatformAvailableSubscriptionPlansService availableSubscriptionPlansService) : base(logger)
    {
        _availableSubscriptionPlansService = availableSubscriptionPlansService;
    }

    [HttpPost]
    [Produces(typeof(PlatformSubscriptionPlanVm))]
    public IActionResult Post([FromBody] DeactivatedSubscriptionPlanIm deactivatedSubscriptionPlanIm)
    {
        return ReturnOkResult(() => _availableSubscriptionPlansService.DectivatedSubscriptionPlan(deactivatedSubscriptionPlanIm));
    }
}