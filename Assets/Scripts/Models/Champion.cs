using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that represents the champion on the board. Inherits from UnitData to get basics, but adds
/// on values important for representing champion entities on the board (e.g level, items, etc)
/// </summary>
public class Champion : UnitData
{
    public int starLevel { get; set; } = 1;
    protected List<Item> items;

    public Champion(int starLevel, UnitData unitData)
    {
        this.starLevel = starLevel;
        this.UnitName = unitData.UnitName;
        this.DisplayName = unitData.DisplayName;
        this.DatabaseID = unitData.DatabaseID;
        this.UnitTraits = unitData.UnitTraits;
        this.Cost = unitData.Cost;
        this.ShopIconName = unitData.ShopIconName;
        this.ChampionIconName = unitData.ChampionIconName;
        this.items = new List<Item>();
    }

    public int getSellPrice()
    {
        if (starLevel == 1)
        {
            return Cost;
        }
        else if (Cost == 1)
        {
            return (int)Math.Pow(3, starLevel - 1) * Cost;
        }
        else
        {
            return (int)Math.Pow(3, starLevel - 1) * Cost - 1;
        }
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public bool canItemBePlaced()
    {
        int fullItemCount = 0;
        foreach (Item item in items)
        {
            fullItemCount += !item.isComponent ? 1 : 0;
        }
        return fullItemCount >= 3;
    }

    public void addItem(Item newItem)
    {
        if (newItem.isComponent)
        {
            int index = 0;
            Item newMergedItem = null;
            foreach (Item item in items)
            {
                if (item.isComponent)
                {
                    newMergedItem = new Item(item.combineItem(newItem));
                    break;
                }
                index++;
            }
            if (newMergedItem != null)
            {
                items.RemoveAt(index);
                items.Add(newMergedItem);
            }
            else
            {
                items.Add(newItem);
            }
        }
        else
        {
            if (items.Count < 3)
            {
                items.Add(newItem);
            }
        }
        foreach (Item item in items)
        {
            Debug.Log(item.isComponent);
        }
    }
}