using SQLite;

public class XPLevelsDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int XPLevelsId { get; set; }
    public int Level { get; set; }
    public int XPRequirement { get; set; }
}