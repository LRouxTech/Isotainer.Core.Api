namespace Isotainer.Module.Wash.Core.ViewModels.WashType;

public record WashTypeListResponse(List<WashTypeItem> WashTypes);

public record WashTypeItem(Guid WashTypeId, string Type, double Cost);