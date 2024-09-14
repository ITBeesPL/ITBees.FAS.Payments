using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

public class ModifiedSubscriptionController : RestfulControllerBase<ModifiedSubscriptionController>
{
    private readonly IModifiedSubscriptionService _modifiedSubscriptionService;

    public ModifiedSubscriptionController(ILogger<ModifiedSubscriptionController> logger,
        IModifiedSubscriptionService modifiedSubscriptionService) : base(logger)
    {
        _modifiedSubscriptionService = modifiedSubscriptionService;
    }

    [HttpGet]
    public IActionResult Post(Guid companyGuid, DateTime validTo, string authKey)
    {
        return ReturnOkResult(() => _modifiedSubscriptionService.Modify(companyGuid, validTo, authKey));
    }
}