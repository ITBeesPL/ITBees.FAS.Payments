using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments;

[Authorize(Roles = Role.PlatformOperator)]
public class PlanFeatureController : RestfulControllerBase<PlanFeatureController>
{
    private readonly IPlanFeatureService _planFeatureService;

    public PlanFeatureController(ILogger<PlanFeatureController> logger,IPlanFeatureService planFeatureService) : base(logger)
    {
        _planFeatureService = planFeatureService;
    }

    [HttpGet]
    [Produces<PlanFeatureVm>]
    public IActionResult Get(int id)
    {
        return ReturnOkResult(() => _planFeatureService.Get(id));
    }

    [HttpPost]
    [Produces<PlanFeatureVm>]
    public IActionResult Post([FromBody] PlanFeatureIm planFeatureIm)
    {
        return ReturnOkResult(() => _planFeatureService.Create(planFeatureIm));
    }

    [HttpPut]
    [Produces<PlanFeatureVm>]
    public IActionResult Put([FromBody] PlanFeatureUm planFeatureUm)
    {
        return ReturnOkResult(() => _planFeatureService.Update(planFeatureUm));
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        return ReturnOkResult(() => _planFeatureService.Delete(id));
    }
}