namespace Isotainer.Module.Tank.Core.ViewModels.WashStatus;

public record WashStatusListResponse(List<WashStatusItem> Items);

public class WashStatusItem(Guid washStatusId, string name);