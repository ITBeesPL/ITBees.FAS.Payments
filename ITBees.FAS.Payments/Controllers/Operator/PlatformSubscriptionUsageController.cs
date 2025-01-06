using ITBees.FAS.Payments.Services;
using ITBees.Interfaces.Repository;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers.Operator;

[Authorize(Roles = Role.PlatformOperator)]
public class PlatformSubscriptionUsageController : RestfulControllerBase<PlatformSubscriptionUsageController>
{
    private readonly IPlatformSubscriptionUsageService _platformSubscriptionUsageService;

    public PlatformSubscriptionUsageController(ILogger<PlatformSubscriptionUsageController> logger,
        IPlatformSubscriptionUsageService platformSubscriptionUsageService) : base(logger)
    {
        _platformSubscriptionUsageService = platformSubscriptionUsageService;
    }

    [HttpGet]
    public IActionResult Get(Guid platfromSubscriptionPlanGuid, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        return ReturnOkResult(()=>_platformSubscriptionUsageService.GetUsersOnPlan(platfromSubscriptionPlanGuid, page, pageSize, sortColumn, sortOrder));
    }
}