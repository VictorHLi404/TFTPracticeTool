using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// A class that represents an item that can be attached to a champion or sit on the bench.
/// Contains a reference to its name by enum.
/// </summary>
public class Item
{
    private Component component;
    private CompletedItem item;
    public bool isComponent;

    private static Dictionary<(Component, Component), CompletedItem> itemMapping = DatabaseAPI.getItemMapping();

    public Item(Component? component)
    {
        if (component == null)
        {
            Debug.LogError("Tried to initialize Item with null argument.");
            return;
        }
        this.component = (Component) component;
        isComponent = true;
    }

    public Item(CompletedItem? item)
    {
        if (item == null)
        {
            Debug.LogError("Tried to initialize Item with null argument.");
            return;
        }
        this.item = (CompletedItem) item;
        isComponent = false;
    }

    /// <summary>
    /// In reference to this one, provide the instance of the other component and then return the item they created.
    /// </summary>
    /// <param name="otherComponent"></param>
    /// <returns></returns>
    public CompletedItem? combineItem(Item otherItem)
    {
        if (!isComponent || !otherItem.isComponent)
        {
            Debug.LogError("Cannot combine an item with a component.");
        }
        Component otherComponent = otherItem.component;
        if (itemMapping.ContainsKey((component, otherComponent)))
        {
            return itemMapping[(component, otherComponent)];
        }
        else if (itemMapping.ContainsKey((otherComponent, component)))
        {
            return itemMapping[(otherComponent, component)];
        }
        else
        {
            Debug.LogError("There is no valid combination for these two components.");
            return null;
        }
    }

    public bool canCombine(Item otherItem)
    {
        if (!isComponent || !otherItem.isComponent)
        {
            return false;
        }
        return true;
    }

    public string getImageString()
    {
        if (isComponent)
        {
            return $"TFT_Item_{component}";
        }
        else
        {
            return $"TFT_Item_{item}";
        }
    }
}