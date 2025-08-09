/// <summary>
/// An abstract class that represents common methods used by both the Bench and ActiveBoard.
/// </summary>
public abstract class UnitManager
{

    public int maxUnitCount;
    public int currentUnitCount = 0;


    public abstract bool CanUnitBePlaced(bool isOnSameManager);

    public abstract void AddUnit();

    public abstract void RemoveUnit();

}