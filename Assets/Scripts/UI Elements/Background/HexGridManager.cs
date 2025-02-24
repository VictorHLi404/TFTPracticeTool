using System.Diagnostics;
using UnityEngine;

public class HexGridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public float tileWidth = 50f;  // Exact width of the hex tile
    public float tileHeight = 50f; // Exact height of the hex tile
    public int rows = 4;            // Number of rows in the grid
    public int columns = 7;         // Number of columns in the grid

    [Header("Spacing Settings")]
    public float spacing = 20f;    // Consistent spacing for both horizontal and vertical gaps

    [Header("Tile Settings")]
    public GameObject tileToGenerate; // Prefab for the tile

    [Header("Alignment Settings")]
    public float verticalShift = 0f; // Vertical shift to move the grid upwards

    private void Start()
    {
        if (tileToGenerate == null)
        {
            UnityEngine.Debug.LogError("Tile prefab not assigned. Please assign it in the inspector.");
            return;
        }

        GenerateHexGrid();
    }

    private void GenerateHexGrid()
    {
        // Add spacing to offsets
        float xOffset = tileWidth + spacing;  // Horizontal spacing
        float yOffset = (tileHeight * 0.75f) + spacing; // Vertical spacing (75% height for staggered rows)

        // Calculate grid width and height
        float gridWidth = (columns - 1) * xOffset + tileWidth; // Total grid width
        float gridHeight = (rows - 1) * yOffset + tileHeight;  // Total grid height

        // Offset to center the grid
        float gridOriginX = transform.position.x;
        float gridOriginY = transform.position.y;
        /*float gridOriginX = -gridWidth / 50f + (tileWidth / 50f);
        float gridOriginY = -gridHeight / 50f + (tileHeight / 50f) + verticalShift;*/

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate the position of each hexagon
                float xPos = col * xOffset + gridOriginX;
                float yPos = row * yOffset + gridOriginY;

                // Shift odd rows to the left
                if (row % 2 == 1)
                {
                    xPos -= xOffset / 2f; // Shift left for staggered rows
                }

                // Instantiate the tile at the calculated position
                GameObject newTile = Instantiate(tileToGenerate, new Vector3(xPos, yPos, 0), Quaternion.identity, transform);

                // Optionally, add customization (e.g., color) here
            }
        }
    }
}
