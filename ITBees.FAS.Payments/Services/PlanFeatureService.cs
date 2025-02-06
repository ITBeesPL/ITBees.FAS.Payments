using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Repository;
using ITBees.Models.Payments;

namespace ITBees.FAS.Payments.Services;

class PlanFeatureService : IPlanFeatureService
{
    private readonly IWriteOnlyRepository<PlanFeature> _planFeatureRwRepo;
    private readonly IReadOnlyRepository<PlanFeature> _planFeatureRoRepo;

    public PlanFeatureService(IWriteOnlyRepository<PlanFeature> planFeatureRwRepo,
        IReadOnlyRepository<PlanFeature> planFeatureRoRepo)
    {
        _planFeatureRwRepo = planFeatureRwRepo;
        _planFeatureRoRepo = planFeatureRoRepo;
    }
    public PlanFeatureVm Get(int id)
    {
        return new PlanFeatureVm(_planFeatureRoRepo.GetData(x => x.Id == id).FirstOrDefault());
    }

    public PlanFeatureVm Create(PlanFeatureIm planFeatureIm)
    {
        var pf = new PlanFeature()
        {
            IsActive = planFeatureIm.IsActive,
            Description = planFeatureIm.Description,
            IsAvailable = planFeatureIm.IsAvailable,
            PlatformFeatureId = planFeatureIm.PlatformFeatureId,
            PlatformSubscriptionPlanGuid = planFeatureIm.PlatformSubscriptionPlanGuid,
            Position = planFeatureIm.Position,
        };
        var result = _planFeatureRwRepo.InsertData(pf);
        return new PlanFeatureVm(_planFeatureRoRepo.GetData(x => x.Id == result.Id,x=>x.PlatformFeature,x=>x.PlatformSubscriptionPlan).First());
    }

    public PlanFeatureVm Update(PlanFeatureUm planFeatureIm)
    {
        var result = _planFeatureRwRepo.UpdateData(x=>x.Id == planFeatureIm.Id,
            x =>
            {
                x.IsActive = planFeatureIm.IsActive;
                x.Description = planFeatureIm.Description;
                x.IsAvailable = planFeatureIm.IsAvailable;
                x.PlatformFeatureId = planFeatureIm.PlatformFeatureId;
                x.Position = planFeatureIm.Position;
                x.PlatformSubscriptionPlanGuid = planFeatureIm.PlatformSubscriptionPlanGuid;
            });

        return new PlanFeatureVm(_planFeatureRoRepo.GetData(x => x.Id == result.First().Id, x => x.PlatformFeature, x => x.PlatformSubscriptionPlan).First());
    }

    public void Delete(int id)
    {
        _planFeatureRwRepo.DeleteData(x => x.Id == id);
    }
}