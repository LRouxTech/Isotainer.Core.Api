namespace Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;

public record IsotainerTankResponse(Guid IsotainerTankId, string TankNumber, Guid WashTypeId, Guid CompanyId, DateTime LoadedOn);