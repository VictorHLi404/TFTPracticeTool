using UnityEngine;

public class HexInteraction : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // TODO: IMPLEMENT A COLLISION FEATURE WHERE THE HEX SHOULD "KNOW" WHEN A CHAMPION HAS BEEN PLACED IN SLOT AND CAN STORE ITS DATA ACCORDINGLY
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this object.");
        }
    }

    private void OnMouseEnter() {
        // Change color on hover
        if (spriteRenderer != null) {
            spriteRenderer.color = Color.yellow; // Highlight color
        }
    }

    private void OnMouseExit() {
        // Revert color when the mouse leaves
        if (spriteRenderer != null) {
            spriteRenderer.color = Color.black; // Default color
        }
    }

    private void OnMouseDown() {
        if (spriteRenderer != null) {
            spriteRenderer.color = Color.white; // Interacted color
        }
    }

    private void OnMouseUp() {
        if (spriteRenderer != null) {
            spriteRenderer.color = Color.yellow; // Default
        }
    }
}
