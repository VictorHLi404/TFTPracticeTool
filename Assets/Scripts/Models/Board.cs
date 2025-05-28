/// <summary>
/// A class that represents the main board. Takes in arguments to determine the current max unit count,
/// and adds some functionality to determine trait levels of the current board.
/// </summary>
public class Board : UnitManager
{
    public Board()
    {
        maxUnitCount = 28;
    }

    public override void AddUnit()
    {
        currentUnitCount++;
    }

    public override void RemoveUnit()
    {
        currentUnitCount--;
    }

    public override bool CanUnitBePlaced()
    {
        return currentUnitCount + 1 <= maxUnitCount;
    }
}