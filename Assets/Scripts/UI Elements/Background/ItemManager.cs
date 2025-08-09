using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemManager : MonoBehaviour
{
    [Header("Item Bench Settings")]
    public int itemSlots = 20;         // Number of bench slots
    public float slotWidth = 1.25f;    // Width of each slot
    public float slotHeight = 1.25f;   // Height of each slot
    public float spacing = 0.375f;      // Spacing between bench slots
    public GameObject itemSlotPrefab; // Prefab for the bench slots.
    public GameObject itemPrefab;

    private ItemBench itemBench;

    private List<GameObject> itemSlotObjects = new List<GameObject>();
    private List<Component> initialItems;

    private List<(float x, float y)> itemSlotCoordinates = new List<(float x, float y)>();

    private void Awake()
    {
        if (itemSlotPrefab == null)
        {
            Debug.LogError("Item slot prefab not loaded in properly.");
        }
        if (itemPrefab == null)
        {
            Debug.LogError("Item prefab not loaded properly.");
        }
        this.itemBench = new ItemBench();
        GenerateBenchElements();
    }

    private void Start()
    {
        if (StartingResources.Instance == null)
        {
            Debug.LogError("Starting Resources instance not identified properly.");
        }
        initialItems = StartingResources.Instance.initialComponents;
        GenerateItems(initialItems);
    }

    /// <summary>
    /// When placing or removing an item from the bench, push all items to the top like a queue;
    /// Remove all items from the bench, then manually replace them. MAINTAIN ORDER!
    /// </summary>
    public void ReshuffleBench()
    {
        List<GameObject> itemsOnBench = new List<GameObject>();
        // re initialize all of the items, prolly ineffficnet but should be ok 
        foreach (GameObject itemSlotObject in itemSlotObjects)
        {
            ItemSlot itemSlot = itemSlotObject.GetComponent<ItemSlot>();
            if (!itemSlot.isEmpty())
            {
                GameObject itemEntity = itemSlot.itemEntityInSlot.gameObject;
                itemsOnBench.Add(itemEntity);
            }
            itemSlot.removeItemFromSlot();
        }
        int index = 0;
        foreach (GameObject itemObject in itemsOnBench)
        {
            ItemEntity itemEntity = itemObject.GetComponent<ItemEntity>();
            itemEntity.Initialize(itemEntity.item, itemSlotObjects[index]);
            itemObject.transform.localPosition = new Vector3(itemSlotCoordinates[index].Item1, itemSlotCoordinates[index].Item2, -1);
            itemSlotObjects[index].GetComponent<ItemSlot>().placeItemInSlot(itemObject.GetComponent<ItemEntity>());
            index++;
        }
    }

    private void GenerateBenchElements()
    {
        float slotSpacing = slotHeight + spacing;
        // needs a big refactor as well holy
        for (int i = 0; i < itemSlots / 2; i++) // first row
        {

            float yPos = 4.75f + -i * slotSpacing;

            GameObject newItemSlot = Instantiate(itemSlotPrefab, transform);
            newItemSlot.GetComponent<ItemSlot>().Initialize(itemBench, this);
            newItemSlot.transform.localPosition = new Vector3(-0.75f, yPos, 0);
            itemSlotObjects.Add(newItemSlot);
            itemSlotCoordinates.Add((-0.75f, yPos));
        }
        for (int i = 0; i < itemSlots / 2; i++)
        {
            float yPos = 4.75f + -i * slotSpacing;

            GameObject newItemSlot = Instantiate(itemSlotPrefab, transform);
            newItemSlot.GetComponent<ItemSlot>().Initialize(itemBench, this);
            newItemSlot.transform.localPosition = new Vector3(0.6f, yPos, 0);
            itemSlotObjects.Add(newItemSlot);
            itemSlotCoordinates.Add((0.6f, yPos));
        }
        Debug.Log($"FINAL LIST LENGTH {itemSlotObjects.Count} + {itemSlotCoordinates.Count}");
    }

    public void ReturnItemsToBench(List<Item> itemList)
    {
        int index = 0;
        for (int i = 0; i < itemSlots; i++)
        {
            if (itemSlotObjects[i].GetComponent<ItemSlot>().isEmpty())
            {
                break;
            }
            index++;
        }
        foreach (Item item in itemList)
        {
            GameObject newItemEntity = Instantiate(itemPrefab, transform);
            newItemEntity.GetComponent<ItemEntity>().Initialize(item, itemSlotObjects[index]);
            newItemEntity.transform.localPosition = new Vector3(itemSlotCoordinates[index].Item1, itemSlotCoordinates[index].Item2, -1);
            itemSlotObjects[index].GetComponent<ItemSlot>().placeItemInSlot(newItemEntity.GetComponent<ItemEntity>());
        }
        ReshuffleBench();
    }

    public List<Component> GetStartingComponents()
    {
        return initialItems;
    }

    private void GenerateItems(List<Component> items)
    {
        if (items.Count > itemSlots)
        {
            Debug.LogError(items.Count);
            Debug.LogError(itemSlots);
            Debug.LogError("Cannot generate a list of items of longer length than designated item count");
        }
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            GameObject newItem = Instantiate(itemPrefab, transform);
            newItem.GetComponent<ItemEntity>().Initialize(new Item(item), itemSlotObjects[i]);
            Debug.Log($"THIS IS WHERE THE ITEM IS BEING SPAWNED: {itemSlotCoordinates[i].Item1} {itemSlotCoordinates[i].Item2}");
            newItem.transform.localPosition = new Vector3(itemSlotCoordinates[i].Item1, itemSlotCoordinates[i].Item2, -1);
            itemSlotObjects[i].GetComponent<ItemSlot>().placeItemInSlot(newItem.GetComponent<ItemEntity>());

        }
    }
}