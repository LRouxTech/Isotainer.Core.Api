namespace Isotainer.Module.Wash.Helpers.Cache;

public static class CacheKeys
{
    public static class WashInstruction
    {
        public const string Tag = "wash-instruction";
        public const string TotalWashesBooked = "wash-instruction:total-records";
        
        public static string Page(int pageIndex, int pageSize, string? search) =>
            $"wash-instruction:page-{pageIndex}:size-{pageSize}:search-{search}";
        
        public static string CompletedInstructions(Guid isotainerTankId, DateTime? from) =>
            $"wash-instruction:completed-isotainerTankId-{isotainerTankId}:from-{from}";
    }

    public static class WashType
    {
        public const string Tag = "wash-type";
        public const string TotalRecords = "wash-type:total-records";
        public const string LastUpdated = "wash-type:last-updated";

        public static string Page(int pageIndex, int pageSize, string? search) =>
            $"wash-type:page-{pageIndex}:size-{pageSize}:search-{search}";
    }
}