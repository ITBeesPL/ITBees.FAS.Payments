using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Roles;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

[Authorize(Roles = Role.PlatformOperator)]
public class PlatformFeatureController : RestfulControllerBase<PlatformFeatureController>
{
    private readonly IPlatformFeautreService _platformFeautreService;

    public PlatformFeatureController(ILogger<PlatformFeatureController> logger,
        IPlatformFeautreService platformFeautreService) : base(logger)
    {
        _platformFeautreService = platformFeautreService;
    }


    [Produces(typeof(PlatformFeatureVm))]
    public IActionResult Post([FromBody] PlatformFeatureIm platformFeatureIm)
    {
        return ReturnOkResult(()=>_platformFeautreService.Create(platformFeatureIm));
    }

    [Produces(typeof(PlatformFeatureVm))]
    public IActionResult Get(int id)
    {
        return ReturnOkResult(() => _platformFeautreService.Get(id));
    }

    [Produces(typeof(PlatformFeatureVm))]
    public IActionResult Put([FromBody] PlatformFeatureUm platformFeatureUm)
    {
        return ReturnOkResult(() => _platformFeautreService.Update(platformFeatureUm));
    }

    public IActionResult Delete(int id)
    {
        return ReturnOkResult(() => _platformFeautreService.Delete(id));
    }
}