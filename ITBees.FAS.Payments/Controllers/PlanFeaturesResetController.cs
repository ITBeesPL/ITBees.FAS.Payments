using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

[Authorize(Roles = Role.PlatformOperator)]
public class PlanFeaturesResetController : RestfulControllerBase<PlanFeaturesResetController>
{
    private readonly IReassignSubscriptionPlansToAvailablePlatformPlans _reassignSubscriptionPlansToAvailablePlatformPlans;

    public PlanFeaturesResetController(ILogger<PlanFeaturesResetController> logger,
        IReassignSubscriptionPlansToAvailablePlatformPlans reassignSubscriptionPlansToAvailablePlatformPlans) : base(logger)
    {
        _reassignSubscriptionPlansToAvailablePlatformPlans = reassignSubscriptionPlansToAvailablePlatformPlans;
    }

    /// <summary>
    /// This method will delete all platform features assigned to subscription plans, and for each subscription will add all available features on platform. Thanks to this You can only edit 'isActive' flag
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Post()
    {
        return ReturnOkResult(() => _reassignSubscriptionPlansToAvailablePlatformPlans.Reassign());
    }
}