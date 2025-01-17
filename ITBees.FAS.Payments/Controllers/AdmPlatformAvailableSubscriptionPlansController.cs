using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

[Authorize(Roles = Role.PlatformOperator)]
public class AdmPlatformAvailableSubscriptionPlansController : RestfulControllerBase<AdmPlatformAvailableSubscriptionPlansController>
{
    private readonly IPlatformAvailableSubscriptionPlansService _availableSubscriptionPlansService;

    public AdmPlatformAvailableSubscriptionPlansController(ILogger<AdmPlatformAvailableSubscriptionPlansController> logger, IPlatformAvailableSubscriptionPlansService availableSubscriptionPlansService) : base(logger)
    {
        _availableSubscriptionPlansService = availableSubscriptionPlansService;
    }

    [HttpGet]
    [Produces(typeof(List<PlatformSubscriptionPlanAdmVm>))]
    public IActionResult Get([FromHeader(Name = "Accept-Language")] string acceptLanguage)
    {
        return ReturnOkResult(() => _availableSubscriptionPlansService.GetAllActivePlans(acceptLanguage));
    }
}