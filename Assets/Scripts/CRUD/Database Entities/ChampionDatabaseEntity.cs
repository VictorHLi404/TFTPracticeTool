using System;
using SQLite;

public class ChampionDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int ChampionId { get; set; }
    public int DatabaseId { get; set; }
    [Column("ChampionEnum")]
    public string ChampionEnumString { get; set; }
    [Ignore]
    public ChampionEnum ChampionEnum
    {
        get => Enum.TryParse(ChampionEnumString, out ChampionEnum result) ? result : throw new Exception("Cannot parse ChampionEnum");
        set => ChampionEnumString = value.ToString();
    }
    public string ChampionName { get; set; }
    public int Cost { get; set; }
    public string Trait1 { get; set; }
    public string Trait2 { get; set; }
    public string Trait3 { get; set; }
    public string ShopIconName { get; set; }
    public string ChampionIconName { get; set; }
}