using Isotainer.Module.Tank.Core.Interfaces.Services;
using Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Tank.Infrastructure.Services;

public class IsotainerTankService : IIsotainerTankService
{
    public Result<IsotainerTankResponse> CreateIsotainerTank(CreateIsotainerTankRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<IsotainerTankResponse> UpdateIsotainerTank(UpdateIsotainerTankRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<IsotainerTankListResponse> GetIsotainerTanks()
    {
        throw new NotImplementedException();
    }

    public Result<bool> ArchiveIsotainerTank(ArchiveIsotainerRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<IsotainerTankResponse> ChangeWashStatus(ChangeWashStatusRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<IsotainerTankResponse> UnloadTank(UnloadTankRequest request)
    {
        throw new NotImplementedException();
    }
}