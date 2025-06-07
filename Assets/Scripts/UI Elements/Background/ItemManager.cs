using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemManager : MonoBehaviour
{
    [Header("Item Bench Settings")]
    public int itemSlots = 10;         // Number of bench slots
    public float slotWidth = 1.25f;    // Width of each slot
    public float slotHeight = 1.25f;   // Height of each slot
    public float spacing = 0.375f;      // Spacing between bench slots
    public GameObject itemSlotPrefab; // Prefab for the bench slots.
    public GameObject itemPrefab;

    private ItemBench itemBench;

    private List<GameObject> itemSlotObjects = new List<GameObject>();

    private List<(float x, float y)> itemSlotCoordinates = new List<(float x, float y)>();

    private void Start()
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
        GenerateTestItems();
    }

    /// <summary>
    /// When placing or removing an item from the bench, push all items to the top like a queue;
    /// Remove all items from the bench, then manually replace them. MAINTAIN ORDER!
    /// </summary>
    public void reshuffleBench()
    {
        Debug.Log("START THE RESHUFFLE");
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
        Debug.Log($"{itemsOnBench.Count}");
        int index = 0;
        foreach (GameObject itemObject in itemsOnBench)
        {
            ItemEntity itemEntity = itemObject.GetComponent<ItemEntity>();
            Debug.Log(itemEntity.item);
            Debug.Log($"{itemSlotCoordinates[index].Item1}, {itemSlotCoordinates[index].Item2}");
            itemEntity.Initialize(itemEntity.item, itemSlotObjects[index]);
            itemObject.transform.localPosition = new Vector3(itemSlotCoordinates[index].Item1, itemSlotCoordinates[index].Item2, -1);
            itemSlotObjects[index].GetComponent<ItemSlot>().placeItemInSlot(itemObject.GetComponent<ItemEntity>());
            index++;
        }
    }

    private void GenerateBenchElements()
    {
        float slotSpacing = slotHeight + spacing;

        for (int i = 0; i < itemSlots; i++)
        {

            float yPos = 4.75f + -i * slotSpacing;

            GameObject newItemSlot = Instantiate(itemSlotPrefab, transform);
            newItemSlot.GetComponent<ItemSlot>().Initialize(itemBench, this);
            newItemSlot.transform.localPosition = new Vector3(-0.345f, yPos, 0);
            itemSlotObjects.Add(newItemSlot);
            itemSlotCoordinates.Add((-0.345f, yPos));
        }
    }

    private void GenerateTestItems()
    {
        GameObject test1 = Instantiate(itemPrefab, transform);
        test1.GetComponent<ItemEntity>().Initialize(new Item(TFTEnums.Component.BFSword), itemSlotObjects[0]);
        test1.transform.localPosition = new Vector3(itemSlotCoordinates[0].Item1, itemSlotCoordinates[0].Item2, -1);
        itemSlotObjects[0].GetComponent<ItemSlot>().placeItemInSlot(test1.GetComponent<ItemEntity>());

        GameObject test2 = Instantiate(itemPrefab, transform);
        test2.GetComponent<ItemEntity>().Initialize(new Item(TFTEnums.Component.NeedlesslyLargeRod), itemSlotObjects[1]);
        test2.transform.localPosition = new Vector3(itemSlotCoordinates[1].Item1, itemSlotCoordinates[1].Item2, -1);
        itemSlotObjects[1].GetComponent<ItemSlot>().placeItemInSlot(test2.GetComponent<ItemEntity>());

        GameObject test3 = Instantiate(itemPrefab, transform);
        test3.GetComponent<ItemEntity>().Initialize(new Item(TFTEnums.Component.RecurveBow), itemSlotObjects[2]);
        test3.transform.localPosition = new Vector3(itemSlotCoordinates[2].Item1, itemSlotCoordinates[2].Item2, -1);
        itemSlotObjects[2].GetComponent<ItemSlot>().placeItemInSlot(test3.GetComponent<ItemEntity>());
    }
}