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
    public bool isBenchSlot;
    public float xCenter, yCenter;
    [Header("Hex Dimensions")]
    public float hexWidth;

    public Champion championInSlot;

    public void Start()
    {
        this.xCenter = transform.position.x;
        this.yCenter = transform.position.y;
    }

    public void Initialize(bool isBenchSlot)
    {
        this.isBenchSlot = isBenchSlot;
    }


}