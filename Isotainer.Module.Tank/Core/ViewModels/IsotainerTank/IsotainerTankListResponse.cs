namespace Isotainer.Module.Tank.Core.ViewModels.IsotainerTank;

public record IsotainerTankListResponse(List<IsotainerTankItem> IsotainerTankItems);

public class IsotainerTankItem(Guid IsotainerTankId, string TankNumber, Guid  CompanyId, Guid WashStatusId);