using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManger : MonoBehaviour
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

    private void GenerateBenchElements()
    {
        float slotSpacing = slotHeight + spacing;

        for (int i = 0; i < itemSlots; i++)
        {

            float yPos = 4.75f + -i * slotSpacing;

            GameObject newItemSlot = Instantiate(itemSlotPrefab, transform);
            newItemSlot.GetComponent<ItemSlot>().Initialize(itemBench);
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
        itemSlotObjects[1].GetComponent<ItemSlot>().placeItemInSlot(test1.GetComponent<ItemEntity>());

        GameObject test3 = Instantiate(itemPrefab, transform);
        test3.GetComponent<ItemEntity>().Initialize(new Item(TFTEnums.Component.RecurveBow), itemSlotObjects[2]);
        test3.transform.localPosition = new Vector3(itemSlotCoordinates[2].Item1, itemSlotCoordinates[2].Item2, -1);
        itemSlotObjects[2].GetComponent<ItemSlot>().placeItemInSlot(test1.GetComponent<ItemEntity>());
    }
}