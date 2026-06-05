namespace Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;

public record CreateIsotainerTankRequest(Guid IsotainerTankId, string TankNumber, Guid CompanyId);