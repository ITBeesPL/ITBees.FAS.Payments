using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers.Operator;

[Authorize(Roles = Role.PlatformOperator)]
public class ApplySubscriptionPlanController : RestfulControllerBase<ApplySubscriptionPlanController>
{
    private readonly IApplySubscriptionPlanAsPlatformOperatorService _applySubscriptionPlanAsPlatformOperatorService;
    public IApplySubscriptionPlanAsPlatformOperatorService ApplySubscriptionPlanToCompanyService { get; }

    public ApplySubscriptionPlanController(
        IApplySubscriptionPlanAsPlatformOperatorService applySubscriptionPlanAsPlatformOperatorService,
        ILogger<ApplySubscriptionPlanController> logger) : base(logger)
    {
        _applySubscriptionPlanAsPlatformOperatorService = applySubscriptionPlanAsPlatformOperatorService;
    }

    [HttpPost]
    [Produces<ApplySubscriptionPlanResultVm>]
    public IActionResult Post([FromBody] ApplySubscriptionPlanToCompanyIm applySubscriptionPlanToCompanyIm)
    {
        // Only the platform operator gets here (class attribute), so a used trial may be applied again - e.g. when a
        // complaint is accepted.
        return ReturnOkResult(() => _applySubscriptionPlanAsPlatformOperatorService.Apply(applySubscriptionPlanToCompanyIm, true));
    }

    [HttpDelete]
    public IActionResult Delete(Guid companyGuid)
    {
        return ReturnOkResult(() => _applySubscriptionPlanAsPlatformOperatorService.Delete(companyGuid));
    }
}