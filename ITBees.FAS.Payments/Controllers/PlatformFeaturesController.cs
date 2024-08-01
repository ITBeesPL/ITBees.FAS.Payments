using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

public class PlatformFeaturesController : RestfulControllerBase<PlatformFeaturesController>
{
    private readonly IPlatformFeatureService _platformFeatureService;

    public PlatformFeaturesController(ILogger<PlatformFeaturesController> logger,
        IPlatformFeatureService platformFeatureService) : base(logger)
    {
        _platformFeatureService = platformFeatureService;
    }

    [HttpGet]
    [Produces<List<PlatformFeatureVm>>]
    public IActionResult Get()
    {
        return ReturnOkResult(() => _platformFeatureService.GetAll());
    }
}