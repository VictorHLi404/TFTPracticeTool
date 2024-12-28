using UnityEngine;

public class BenchManager : MonoBehaviour
{
    [Header("Bench Settings")]
    public int benchSlots = 9;         // Number of bench slots
    public float slotWidth = 1.0f;    // Width of each slot
    public float slotHeight = 1.0f;   // Height of each slot
    public float spacing = 0.2f;      // Spacing between bench slots
    public float verticalOffset = 1.2f; // Vertical offset from the grid (shifted slightly up)
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
        float benchOriginX = -benchWidth / 2f + (slotWidth / 2f);
        float benchOriginY = -verticalOffset; // Vertical offset (adjusted upwards)

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
