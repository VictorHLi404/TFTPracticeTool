using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayIcon : MonoBehaviour
{
    public void Initialize(AllItemsEnum item)
    {
        Image itemIcon = gameObject.GetComponent<Image>();
        CompletedItem completedItem;
        if (!Enum.TryParse(item.ToString(), out completedItem))
            Debug.LogError($"Failed to parse {item} into a CompletedItemEnum");

        string itemName = new Item(completedItem).getImageString();
        string file_path = $"ItemIcons/{itemName}";
        itemIcon.sprite = Resources.Load<Sprite>(file_path);
    }
}