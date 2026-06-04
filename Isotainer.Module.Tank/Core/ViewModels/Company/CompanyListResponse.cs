namespace Isotainer.Module.Tank.Core.ViewModels.Company;

public record CompanyListResponse(List<CompanyItem> Items);

public class CompanyItem(Guid companyId, string name);

