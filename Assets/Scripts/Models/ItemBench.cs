using UnityEngine.UIElements;
using UnityEngine;

public class ItemBench
{

    private int maxItemCount = 20;
    private int currentItemCount = 0;

    public ItemBench()
    {
        maxItemCount = 20;
        currentItemCount = 0;
    }

    public void AddItem()
    {
        currentItemCount++;
        Debug.Log($"THIS IS THE CURRENT ITEM BENCH COUNT {currentItemCount}");
    }

    public void RemoveItem()
    {
        currentItemCount--;
        Debug.Log($"THIS IS THE CURRENT ITEM BENCH COUNT {currentItemCount}");
    }

    public bool CanItemBePlaced()
    {
        return currentItemCount + 1 <= maxItemCount;
    }
}