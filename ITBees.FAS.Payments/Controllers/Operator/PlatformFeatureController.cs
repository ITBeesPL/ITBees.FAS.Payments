using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers.Operator;

[Authorize(Roles = Role.PlatformOperator)]
public class PlatformFeatureController : RestfulControllerBase<PlatformFeatureController>
{
    private readonly IPlatformFeatureService _platformFeatureService;

    public PlatformFeatureController(ILogger<PlatformFeatureController> logger,
        IPlatformFeatureService platformFeatureService) : base(logger)
    {
        _platformFeatureService = platformFeatureService;
    }

    [HttpPost]
    [Produces(typeof(PlatformFeatureVm))]
    public IActionResult Post([FromBody] PlatformFeatureIm platformFeatureIm)
    {
        return ReturnOkResult(()=>_platformFeatureService.Create(platformFeatureIm));
    }

    [HttpGet]
    [Produces(typeof(PlatformFeatureVm))]
    public IActionResult Get(int id)
    {
        return ReturnOkResult(() => _platformFeatureService.Get(id));
    }

    [HttpPut]
    [Produces(typeof(PlatformFeatureVm))]
    public IActionResult Put([FromBody] PlatformFeatureUm platformFeatureUm)
    {
        return ReturnOkResult(() => _platformFeatureService.Update(platformFeatureUm));
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        return ReturnOkResult(() => _platformFeatureService.Delete(id));
    }
}