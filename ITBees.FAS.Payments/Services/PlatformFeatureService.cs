using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.RestfulApiControllers.Exceptions;
using ITBees.UserManager.Interfaces.Services;

namespace ITBees.FAS.Payments.Services;

public class PlatformFeatureService : IPlatformFeatureService
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IWriteOnlyRepository<PlatformFeature> _platformFeatureRwRepo;
    private readonly IReadOnlyRepository<PlatformFeature> _platformFeatureRoRepo;

    public PlatformFeatureService(
        IAspCurrentUserService aspCurrentUserService,
        IWriteOnlyRepository<PlatformFeature> platformFeatureRwRepo,
        IReadOnlyRepository<PlatformFeature> platformFeatureRoRepo)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _platformFeatureRwRepo = platformFeatureRwRepo;
        _platformFeatureRoRepo = platformFeatureRoRepo;
    }
    public PlatformFeatureVm Create(PlatformFeatureIm x)
    {
        var pf = new PlatformFeature()
        {
            FeatureName = x.FeatureName,
            FeatureDescription = x.FeatureDescription,
            IsVisible = x.IsVisible, 
            FeatureValueDescription = x.FeatureValueDescription,
            GroupName = x.GroupName,
        };

        var result = _platformFeatureRwRepo.InsertData(pf);
        return new PlatformFeatureVm(result);
    }

    public PlatformFeatureVm Get(int id)
    {
        var platformFeature = _platformFeatureRoRepo.GetData(x => x.Id == id).FirstOrDefault();
        if (platformFeature == null)
        {
            throw new ResultNotFoundException($"Feature with Id = {id}not exists");
        }

        return new PlatformFeatureVm(platformFeature);
    }

    public PlatformFeatureVm Update(PlatformFeatureUm platformFeatureUm)
    {
        var updated = _platformFeatureRwRepo.UpdateData(x => x.Id == platformFeatureUm.Id, x =>
        {
            x.FeatureDescription = platformFeatureUm.FeatureDescription;
            x.FeatureName = platformFeatureUm.FeatureName;
            x.IsVisible = platformFeatureUm.IsVisible;
        }).FirstOrDefault();

        return new PlatformFeatureVm(updated);
    }

    public void Delete(int id)
    {
        _platformFeatureRwRepo.DeleteData(x => x.Id == id);
    }

    public List<PlatformFeatureVm> GetAll()
    {
        var platformFeatureVms = _platformFeatureRoRepo.GetData(x => x.IsVisible).Select(x => new PlatformFeatureVm(x)).ToList();
        return platformFeatureVms;
    }
}