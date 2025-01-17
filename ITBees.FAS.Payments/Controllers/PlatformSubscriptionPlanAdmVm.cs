using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Controllers;

public class PlatformSubscriptionPlanAdmVm : PlatformSubscriptionPlanVm 
{
    public int UsersCount { get; set; }
    public DateTime? StartFrom { get; set; }
}