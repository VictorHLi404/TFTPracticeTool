using SQLite;

public class DefaultBagSizesDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int DefaultBagSizesId { get; set; }
    public int ChampionCost { get; set; }
    public int BagSize { get; set; }
}