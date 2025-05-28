using UnityEngine;


/// <summary>
/// A MonoBehaviour representing a slot which a unit can occupy. Used for both the bench 
/// and for the main grid, containing a boolean that decides whether it contributes to the
/// maxCount of units AND if it contributes to the trait tracker.
///
/// Contains a reference to the champion currently inside of the slot, if there is one.
/// 
/// Holds logic to prevent multiple units from being occupied.
/// </summary>
public class UnitSlot : MonoBehaviour
{
    public UnitManager parentManager; // reference to either a bench slot manager or board
    public bool isBenchSlot;

    [Header("Hex Dimensions")]
    public float hexWidth;

    public Champion championInSlot;

    public void Start()
    {

    }

    public void Initialize(UnitManager parentManager, bool isBenchSlot)
    {
        Debug.Log(parentManager);
        this.parentManager = parentManager;
        this.isBenchSlot = isBenchSlot;
    }

    public bool isEmpty()
    {
        return championInSlot == null;
    }

    public void placeChampionInSlot(ChampionEntity newChampion)
    {
        championInSlot = newChampion.champion;
        parentManager.AddUnit();
    }

    public void removeChampionFromSlot()
    {
        championInSlot = null;
        if (isBenchSlot)
        {
            parentManager.RemoveUnit();
        }
    }

}