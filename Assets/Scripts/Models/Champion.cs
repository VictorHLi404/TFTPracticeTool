/// <summary>
/// A class that represents the champion on the board. Inherits from UnitData to get basics, but adds
/// on values important for representing champion entities on the board (e.g level, items, etc)
/// </summary>
public class Champion : UnitData
{
    public int starLevel { get; set; } = 1;
    protected Item[] items;

    public Champion(int starLevel, UnitData unitData)
    {

    }
}