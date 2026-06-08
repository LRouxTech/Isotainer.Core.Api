using LRouxTech.Core.Auth.Api.Authorization;

namespace Isotainer.Core.Api.Auth;

public class IsotainerPermissions : AppPermissions
{
    public new static class UserManagement
    {
        public static readonly PermissionKey Create = UserManagementSystem.Create;
        public static readonly PermissionKey Read = UserManagementSystem.Read;
        public static readonly PermissionKey Update = UserManagementSystem.Update;
        public static readonly PermissionKey Delete = UserManagementSystem.Delete;
    }
    
    public static class Tank
    {
        public const string CompanySection = "Company";

        public static readonly PermissionKey ViewCompanies = new(CompanySection, nameof(ViewCompanies));
        public static readonly PermissionKey CreateCompany = new(CompanySection, nameof(CreateCompany));
        public static readonly PermissionKey UpdateCompany = new(CompanySection, nameof(UpdateCompany));
        public static readonly PermissionKey DeleteCompany = new(CompanySection, nameof(DeleteCompany));
        
        public const string IsotainerSection = "Isotainer";

        public static readonly PermissionKey ViewIsotainers = new(IsotainerSection, nameof(ViewIsotainers));
        public static readonly PermissionKey CreateIsotainer = new(IsotainerSection, nameof(CreateIsotainer));
        public static readonly PermissionKey UpdateIsotainer = new(IsotainerSection, nameof(UpdateIsotainer));
        public static readonly PermissionKey ChangeIsotainerWashStatus = new(IsotainerSection, nameof(ChangeIsotainerWashStatus));
        public static readonly PermissionKey UnloadIsotainer = new(IsotainerSection, nameof(UnloadIsotainer));
        public static readonly PermissionKey DeleteIsotainer = new(IsotainerSection, nameof(DeleteIsotainer));

        public const string WashStatusSection = "WashStatus";

        public static readonly PermissionKey ViewWashStatuses = new(WashStatusSection, nameof(ViewWashStatuses));

    }
    
    public static class Finance
    {
        public const string GeneralCostSection = "GeneralCost";

        public static readonly PermissionKey ViewGeneralCosts = new(GeneralCostSection, nameof(ViewGeneralCosts));
        public static readonly PermissionKey UpdateGeneralCosts = new(GeneralCostSection, nameof(UpdateGeneralCosts));
        
        public const string InvoiceSection = "Invoice";

        public static readonly PermissionKey ViewTankInvoices = new(InvoiceSection, nameof(ViewTankInvoices));
        public static readonly PermissionKey ViewCompanyInvoices = new(InvoiceSection, nameof(ViewCompanyInvoices));
        public static readonly PermissionKey ViewInvoice = new(InvoiceSection, nameof(ViewInvoice));
        public static readonly PermissionKey GenerateTankInvoice = new(InvoiceSection, nameof(GenerateTankInvoice));

        public const string InvoiceLineSection = "InvoiceLine";

        public static readonly PermissionKey ViewInvoiceLines = new(InvoiceLineSection, nameof(ViewInvoiceLines));
    }
    
    public static class Wash
    {
        public const string WashTypeSection = "WashType";

        public static readonly PermissionKey ViewWashTypes = new(WashTypeSection, nameof(ViewWashTypes));
        public static readonly PermissionKey CreateWashType = new(WashTypeSection, nameof(CreateWashType));
        public static readonly PermissionKey UpdateWashType = new(WashTypeSection, nameof(UpdateWashType));
        public static readonly PermissionKey DeleteWashType = new(WashTypeSection, nameof(DeleteWashType));
        
        public const string WashInstructionSection = "WashInstruction";

        public static readonly PermissionKey ViewWashInstructions = new(WashInstructionSection, nameof(ViewWashInstructions));
        public static readonly PermissionKey CreateWashInstruction = new(WashInstructionSection, nameof(CreateWashInstruction));
        public static readonly PermissionKey UpdateWashInstruction = new(WashInstructionSection, nameof(UpdateWashInstruction));
        public static readonly PermissionKey DeleteWashInstruction = new(WashInstructionSection, nameof(DeleteWashInstruction));

    }
}