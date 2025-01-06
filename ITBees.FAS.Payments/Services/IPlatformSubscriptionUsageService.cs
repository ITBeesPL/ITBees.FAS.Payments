using ITBees.Interfaces.Repository;
using ITBees.UserManager.Controllers.PlatformOperator;

namespace ITBees.FAS.Payments.Services;

public interface IPlatformSubscriptionUsageService
{
    PaginatedResult<PlatformUserAccountVm> GetUsersOnPlan(Guid platfromSubscriptionPlanGuid, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder);
}