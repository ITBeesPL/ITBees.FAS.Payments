using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPlanFeatureService
{
    PlanFeatureVm Get(int id);
    PlanFeatureVm Create(PlanFeatureIm planFeatureIm);
    PlanFeatureVm Update(PlanFeatureUm planFeatureUm);
    void Delete(int id);
}