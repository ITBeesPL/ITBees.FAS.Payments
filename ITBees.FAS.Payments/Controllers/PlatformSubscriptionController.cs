using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

[Authorize(Roles = Role.PlatformOperator)]
public class PlatformSubscriptionController : RestfulControllerBase<PlatformSubscriptionController>
{
    private readonly IPlatformAvailableSubscriptionPlansService _availableSubscriptionPlansService;

    public PlatformSubscriptionController(ILogger<PlatformSubscriptionController> logger, IPlatformAvailableSubscriptionPlansService availableSubscriptionPlansService) : base(logger)
    {
        _availableSubscriptionPlansService = availableSubscriptionPlansService;
    }

    [HttpGet]
    [Produces(typeof(PlatformSubscriptionPlanVm))]
    public IActionResult Get(Guid selectedSubscriptionPlanGuid)
    {
        return ReturnOkResult(() => _availableSubscriptionPlansService.Get(selectedSubscriptionPlanGuid));
    }

    [HttpPost]
    [Produces(typeof(PlatformSubscriptionPlanVm))]
    public IActionResult Post([FromBody] PlatformSubscriptionPlanIm selectedSubscriptionPlanIm)
    {
        return ReturnOkResult(() => _availableSubscriptionPlansService.CreateNew(selectedSubscriptionPlanIm));
    }

    [HttpPut]
    [Produces(typeof(PlatformSubscriptionPlanVm))]
    public IActionResult Put([FromBody] PlatformSubscriptionPlanUm selectedSubscriptionPlanUm)
    {
        return ReturnOkResult(() => _availableSubscriptionPlansService.Update(selectedSubscriptionPlanUm));
    }

    [HttpDelete]
    public IActionResult Del(Guid selectedSubscriptionPlanGuid)
    {
        return ReturnOkResult(() => _availableSubscriptionPlansService.Delete(selectedSubscriptionPlanGuid));
    }
}