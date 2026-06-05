using Isotainer.Module.Wash.Core.Interfaces.Services;
using Isotainer.Module.Wash.Core.ViewModels.WashType;
using LRouxTech.Core.ValidationResult;

namespace Isotainer.Module.Wash.Infrastructure.Services;

public class WashTypeService : IWashTypeService
{
    public Result<WashTypeResponse> CreateWashType(CreateWashTypeRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<WashTypeResponse> UpdateWashType(UpdateWashTypeRequest request)
    {
        throw new NotImplementedException();
    }

    public Result<WashTypeListResponse> ListWashTypes()
    {
        throw new NotImplementedException();
    }

    public Result<bool> ArchiveWashType(ArchiveWashTypeRequest request)
    {
        throw new NotImplementedException();
    }
}