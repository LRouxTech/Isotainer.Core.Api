using Isotainer.Module.Tank.Core.Entities;

namespace Isotainer.Module.Tank.Core.ViewModels.WashStatus;

public record WashStatusListResponse(List<WashStatusItem> Items);

public record WashStatusItem(Guid washStatusId, WashStatusEnum type);