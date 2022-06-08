namespace EnergyPrices.Models;

public class DataItem
{
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    public decimal Value { get; set; }

    public bool Highlighted { get; set; }
}