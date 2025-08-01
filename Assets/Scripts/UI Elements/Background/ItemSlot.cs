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
public class ItemSlot : MonoBehaviour
{
    public ItemBench itemBench; // reference to either a bench slot manager or board
    public ItemManager itemManager;

    [Header("Hex Dimensions")]
    public float hexWidth;

    public ItemEntity itemEntityInSlot;

    public void Start()
    {

    }

    public void Initialize(ItemBench itemBench, ItemManager itemManager)
    {
        this.itemBench = itemBench;
        this.itemManager = itemManager;
    }

    public bool isEmpty()
    {
        return itemEntityInSlot == null;
    }

    public void placeItemInSlot(ItemEntity newItem)
    {
        itemEntityInSlot = newItem;
        itemBench.AddItem();
    }

    public void removeItemFromSlot()
    {
        itemEntityInSlot = null;
        itemBench.RemoveItem();
    }

    public void callReshuffle()
    {
        itemManager.ReshuffleBench();
    }
}
