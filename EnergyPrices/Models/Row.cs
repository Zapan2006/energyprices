namespace EnergyPrices.Models;

public class Row
{
    public Column Columns { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}