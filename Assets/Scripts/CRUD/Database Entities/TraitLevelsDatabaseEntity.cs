using SQLite;

public class TraitLevelsDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int TraitLevelsId { get; set; }

    public string TraitName { get; set; }
    public int? TierOne { get; set; }
    public int? TierTwo { get; set; }
    public int? TierThree { get; set; }
    public int? TierFour { get; set; }
    public int? TierFive { get; set; }
    public int? TierSix { get; set; }
    public int? TierSeven { get; set; }
    public int? TierEight { get; set; }
    public int? TierNine { get; set; }
    public int? TierTen { get; set; }
}