using SQLite;

public class ShopOddsDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int ShopOddsId { get; set; }
    public int Level { get; set; }
    public int OneCostOdds { get; set; }
    public int TwoCostOdds { get; set; }
    public int ThreeCostOdds { get; set; }
    public int FourCostOdds { get; set; }
    public int FiveCostOdds { get; set; }

}