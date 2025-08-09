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
    }

    public void RemoveItem()
    {
        currentItemCount--;
    }

    public bool CanItemBePlaced()
    {
        return currentItemCount + 1 <= maxItemCount;
    }
}