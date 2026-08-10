namespace Isotainer.Module.Tank.Helpers.Cache;

public static class CacheKeys
{
    public static class Company
    {
        public const string Tag = "company";
        public const string TotalRecords = "company:total-records";
        public const string LastUpdated = "company:last-updated";
        public static string Page(int pageIndex, int pageSize, string? search) =>
            $"company:page-{pageIndex}:size-{pageSize}:search-{search}";
    }

    public static class WashStatus
    {
        public const string Tag = "wash-statuses";

        public static string Page(int pageIndex, int pageSize, string? search) =>
            $"wash-statuses:page-{pageIndex}:size-{pageSize}:search-{search}";
    }
    
    public static class IsotainerTank
    {
        public const string Tag = "IsotainerTank";
        public const string TotalActiveTanks = "IsotainerTank:total-active-tanks";
        public const string NewInventory = "IsotainerTank:new-inventory";
        public const string AverageTurnaroundTime = "IsotainerTank:average-turnaround-time";
        
        public static string ById(Guid id) => $"IsotainerTank:id-{id}";
        public static string Page(int pageIndex, int pageSize, string? search) =>
            $"IsotainerTank:page-{pageIndex}:size-{pageSize}:search-{search}";
        
        public static string ByIds = $"IsotainerTank:page-ids";
    }
}