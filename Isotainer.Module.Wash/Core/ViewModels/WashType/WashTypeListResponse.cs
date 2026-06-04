namespace Isotainer.Module.Wash.Core.ViewModels.WashType;

public record WashTypeListResponse(List<WashTypeItem> WashTypes);

public class WashTypeItem(string Type, double Cost);