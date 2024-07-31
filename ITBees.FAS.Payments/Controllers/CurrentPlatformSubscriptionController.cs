using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

public class CurrentPlatformSubscriptionController : RestfulControllerBase<CurrentPlatformSubscriptionController>
{
    private readonly ISubscriptionPlansService _subscriptionPlansService;

    public CurrentPlatformSubscriptionController(ILogger<CurrentPlatformSubscriptionController> logger, ISubscriptionPlansService subscriptionPlansService) : base(logger)
    {
        _subscriptionPlansService = subscriptionPlansService;
    }

    [HttpGet]
    [Produces(typeof(SelectedSubscriptionPlanVm))]
    public IActionResult Get(Guid selectedSubscriptionPlanGuid)
    {
        return ReturnOkResult(() => _subscriptionPlansService.Get(selectedSubscriptionPlanGuid));
    }

    [HttpPost]
    [Produces(typeof(SelectedSubscriptionPlanVm))]
    public IActionResult Post([FromBody] SelectedSubscriptionPlanIm selectedSubscriptionPlanIm)
    {
        return ReturnOkResult(() => _subscriptionPlansService.CreateNew(selectedSubscriptionPlanIm));
    }

    [HttpPut]
    [Produces(typeof(SelectedSubscriptionPlanVm))]
    public IActionResult Put([FromBody] SelectedSubscriptionPlanUm selectedSubscriptionPlanUm)
    {
        return ReturnOkResult(() => _subscriptionPlansService.Update(selectedSubscriptionPlanUm));
    }

    [HttpDelete]
    public IActionResult Del(Guid selectedSubscriptionPlanGuid)
    {
        return ReturnOkResult(() => _subscriptionPlansService.Delete(selectedSubscriptionPlanGuid));
    }
}