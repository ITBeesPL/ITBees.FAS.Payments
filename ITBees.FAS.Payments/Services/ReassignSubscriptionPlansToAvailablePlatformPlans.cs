using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;

namespace ITBees.FAS.Payments.Services;

class ReassignSubscriptionPlansToAvailablePlatformPlans : IReassignSubscriptionPlansToAvailablePlatformPlans
{
    private readonly IWriteOnlyRepository<PlanFeature> _planFeatureRwRepo;
    private readonly IReadOnlyRepository<PlatformSubscriptionPlan> _platformSubscriptionPlanRoRepo;
    private readonly IReadOnlyRepository<PlatformFeature> _platformFeatureRoRepo;

    public ReassignSubscriptionPlansToAvailablePlatformPlans(IWriteOnlyRepository<PlanFeature> planFeatureRwRepo,
        IReadOnlyRepository<PlatformSubscriptionPlan>  platformSubscriptionPlanRoRepo,
        IReadOnlyRepository<PlatformFeature> platformFeatureRoRepo)
    {
        _planFeatureRwRepo = planFeatureRwRepo;
        _platformSubscriptionPlanRoRepo = platformSubscriptionPlanRoRepo;
        _platformFeatureRoRepo = platformFeatureRoRepo;
    }
    public void Reassign()
    {

        _planFeatureRwRepo.DeleteData(x => true);

        var platformFeature = _platformFeatureRoRepo.GetData(x => true).ToList();
        
        var subscriptionPlans = _platformSubscriptionPlanRoRepo.GetData(x => x.IsActive).ToList();
        foreach (var subscriptionPlan in subscriptionPlans)
        {
            var i = 0;
            foreach (var feature in platformFeature)
            {
                i++;
                _planFeatureRwRepo.InsertData(new PlanFeature()
                {
                    IsActive = true,
                    Description = string.Empty,
                    IsAvailable = true,
                    PlatformFeatureId = feature.Id,
                    PlatformSubscriptionPlanGuid = subscriptionPlan.Guid,
                    Position = i
                });
            }
        }
    }
}