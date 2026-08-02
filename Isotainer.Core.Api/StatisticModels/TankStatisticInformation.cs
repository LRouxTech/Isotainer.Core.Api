namespace Isotainer.Core.Api.StatisticModels;

public class TankStatisticInformation
{
    public int TotalActiveTanks { get; set; }
    public int TotalNewInventory { get; set; }
    public int TotalWashesBooked { get; set; }
    public string AverageTurnaroundTime { get; set; }
}