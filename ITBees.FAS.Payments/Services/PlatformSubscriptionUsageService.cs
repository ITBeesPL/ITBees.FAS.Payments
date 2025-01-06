using ITBees.Interfaces.Repository;
using ITBees.Models.Users;
using ITBees.RestfulApiControllers.Exceptions;
using ITBees.UserManager.Controllers.PlatformOperator;
using ITBees.UserManager.Interfaces;

namespace ITBees.FAS.Payments.Services;

public class PlatformSubscriptionUsageService : IPlatformSubscriptionUsageService
{
    private readonly IReadOnlyRepository<UsersInCompany> _usersInCompanyRoRepo;
    private readonly IAspCurrentUserService _aspCurrentUserService;

    public PlatformSubscriptionUsageService(IReadOnlyRepository<UsersInCompany> usersInCompanyRoRepo,
        IAspCurrentUserService aspCurrentUserService)
    {
        _usersInCompanyRoRepo = usersInCompanyRoRepo;
        _aspCurrentUserService = aspCurrentUserService;
    }

    public PaginatedResult<PlatformUserAccountVm> GetUsersOnPlan(Guid platfromSubscriptionPlanGuid, int? page,
        int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        if (_aspCurrentUserService.CurrentUserIsPlatformOperator() == false)
            throw new FasApiErrorException("Current user is not platform operator", 403);

        return _usersInCompanyRoRepo
            .GetDataPaginated(
                x => x.Company.CompanyPlatformSubscription.SubscriptionPlanGuid == platfromSubscriptionPlanGuid,
                new SortOptions(page, pageSize, sortColumn, sortOrder),
                x => x.UserAccount, x => x.Company, x => x.Company.CompanyPlatformSubscription)
            .MapTo(x => new PlatformUserAccountVm(x));
    }
}