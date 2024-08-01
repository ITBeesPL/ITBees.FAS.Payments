using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IPlatformFeautreService
{
    PlatformFeatureVm Create(PlatformFeatureIm platformFeatureIm);
    PlatformFeatureVm Get(int id);
    PlatformFeatureVm Update(PlatformFeatureUm platformFeatureUm);
    void Delete(int id);
}