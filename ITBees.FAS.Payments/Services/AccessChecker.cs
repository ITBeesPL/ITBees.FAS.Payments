using ITBees.Interfaces.Platforms;
using ITBees.RestfulApiControllers.Exceptions;
using ITBees.RestfulApiControllers.Models;
using ITBees.UserManager.Interfaces;

namespace ITBees.FAS.Payments.Services;

public class AccessChecker : IAccessChecker 
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IPlatformSettingsService _platformSettingsService;

    public AccessChecker(IAspCurrentUserService aspCurrentUserService, IPlatformSettingsService platformSettingsService)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _platformSettingsService = platformSettingsService;
    }
    public void CheckAccess(string? authKey)
    {
        if (string.IsNullOrEmpty(authKey) && _aspCurrentUserService.CurrentUserIsPlatformOperator() == false)
        {
            throw new FasApiErrorException(new FasApiErrorVm("Unauthorized access attempt", 401, ""));
        }

        if (authKey == _platformSettingsService.GetSetting("platformAuthKey"))
            return;

        if (_aspCurrentUserService.CurrentUserIsPlatformOperator())
            return;

        throw new FasApiErrorException(new FasApiErrorVm("Unauthorized access attempt", 401, ""));
    }
}