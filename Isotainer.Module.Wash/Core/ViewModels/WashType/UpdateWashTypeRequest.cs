namespace Isotainer.Module.Wash.Core.ViewModels.WashType;

public record UpdateWashTypeRequest(Guid WashTypeId, string Type, double Cost);