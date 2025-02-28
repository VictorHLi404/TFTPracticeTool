using UnityEngine;

public class BenchManager : MonoBehaviour
{
    [Header("Bench Settings")]
    public int benchSlots = 9;         // Number of bench slots
    public float slotWidth = 60f;    // Width of each slot
    public float slotHeight = 60f;   // Height of each slot
    public float spacing = 25f;      // Spacing between bench slots
    public GameObject benchSlotPrefab; // Prefab for the bench slots

    private void Start()
    {
        if (benchSlotPrefab == null)
        {
            Debug.LogError("Bench slot prefab not assigned.");
            return;
        }

        GenerateBench();
    }

    private void GenerateBench()
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

            // Instantiate the bench slot
            Instantiate(benchSlotPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, transform);
        }
    }
}
