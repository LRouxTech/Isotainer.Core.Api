using LRouxTech.Core.Auth.Api.Authorization;

namespace Isotainer.Core.Api.Auth;

public class IsotainerRoles : AppRoles
{
    public new const string Admin = UserManagementRoles.Admin;
    public const string TankAdmin = "TankAdmin";
    public const string FinanceAdmin = "FinanceAdmin";
    public const string Washer = "Washer";

}