using System.Collections.Generic;
using UnityEngine;

public class BenchManager : MonoBehaviour
{
    [Header("Bench Settings")]
    public int benchSlots = 9;         // Number of bench slots
    public float slotWidth = 60f;    // Width of each slot
    public float slotHeight = 60f;   // Height of each slot
    public float spacing = 25f;      // Spacing between bench slots
    public GameObject benchSlotPrefab; // Prefab for the bench slots.

    private Bench bench;
    private List<GameObject> benchSlotObjects;

    private void Start()
    {
        if (benchSlotPrefab == null)
        {
            Debug.LogError("Bench slot prefab not assigned.");
            return;
        }

        GenerateBenchElements();
        this.bench = new Bench();
    }

    private void GenerateBenchElements()
    {
        float slotSpacing = slotWidth + spacing; // Horizontal spacing between slots

        // Calculate total bench width
        float benchWidth = (benchSlots - 1) * slotSpacing + slotWidth;
        float benchOriginX = transform.position.x;
        float benchOriginY = transform.position.y;

        for (int i = 0; i < benchSlots; i++)
        {
            // Calculate the position of each bench slot
            float xPos = i * slotSpacing + benchOriginX;
            float yPos = benchOriginY;

            // Instantiate the bench slot and add to the list
            benchSlotObjects.Add(Instantiate(benchSlotPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, transform));
        }
    }

    /// <summary>
    /// A method that validates whether a champion can be added to the bench or not based off of the total count.
    /// Called by the internal unit slot for drag and drop, and by ShopUI for spawnign in elements.
    /// </summary>
    /// <returns>Whether this action was done sucessfully or not.</returns>
    public bool AddToBench()
    {
        if (bench.CanUnitBePlaced())
        {
            /// add to the internal list
            bench.AddUnit();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// A method that, given a champion object, spawns in an appropriate champion entity object
    /// at the leftmost position on the bench.
    /// </summary>
    /// <returns></returns>
    public bool placeInBench()
    {
        foreach (GameObject benchSlot in benchSlotObjects)
        {
            if (benchSlot.)
        }
    }

    /// <summary>
    /// A method that 
    /// </summary>
    /// <returns></returns>
    public bool removeFromBench()
    {

    }
}
