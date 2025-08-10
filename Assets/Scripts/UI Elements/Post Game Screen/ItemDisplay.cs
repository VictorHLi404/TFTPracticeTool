using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    [Header("Reference Fields")]
    public List<GameObject> Items;
    public List<GameObject> Spacers;

    public void Initialize(List<AllItemsEnum> items)
    {
        int itemsToActivate = Math.Min(items.Count, 3);

        for (int i = 0; i < 3; i++)
        {
            bool isActive = i < itemsToActivate;
            Items[i].SetActive(isActive);
            if (i < 2)
            {
                Spacers[i].SetActive(i < itemsToActivate - 1);
            }
        }
        for (int i = 0; i < itemsToActivate; i++)
        {
            var itemIcon = Items[i].GetComponent<ItemDisplayIcon>();
            itemIcon.Initialize(items[i]);
        }
    }
}