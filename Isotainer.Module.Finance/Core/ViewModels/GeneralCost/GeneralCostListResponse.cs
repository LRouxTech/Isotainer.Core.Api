namespace Isotainer.Module.Finance.Core.ViewModels.GeneralCost;

public record GeneralCostListResponse(List<GeneralCostItem> GeneralCosts);

public record GeneralCostItem(Guid GeneralCostId, string Name, double Cost);