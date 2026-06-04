namespace Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;

public record UpdateIsotainerTankRequest(Guid IsotainerTankId, string TankNumber, Guid CompanyId);