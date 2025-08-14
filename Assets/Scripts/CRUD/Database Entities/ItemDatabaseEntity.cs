using SQLite;

public class ItemDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int ItemId { get; set; }
    public string ComponentOne { get; set; }
    public string ComponentTwo { get; set; }
    public string CompletedItem{ get; set; }
}