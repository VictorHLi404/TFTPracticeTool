using UnityEngine.UIElements;

public class ItemBench
{

    private int maxItemCount = 10;
    private int currentItemCount = 0;

    public ItemBench()
    {
        maxItemCount = 10;
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