using UnityEngine;

public class ChampionInteraction : DragAndDrop 
{
    private SpriteRenderer spriteRenderer; // tool for rendering sprite 
    private GameObject currentLocationCollider = null; // variable to interface with current hex / bench slot that the unit is sitting on
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private new void Start()
    {
        base.Start(); // initialize the stuff 2 do with the drag and drop
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this object.");
        }
    }

    private void OnMouseEnter() { // highlight champion, visual effect
        // Change color on hover TEMPORARY TEST
        if (spriteRenderer != null) {
            spriteRenderer.color = Color.yellow; // Highlight color
        }
    }

    private void OnMouseExit() {
        // Revert color when the mouse leaves
        if (spriteRenderer != null) {
            spriteRenderer.color = Color.white; // Default color
        }
    }

    private void OnCollisionEnter2D(Collision2D collisionObject) {
        Debug.Log("ENTERED A NEW TILE");
        currentLocationCollider = collisionObject.gameObject;
    }

    private void OnCollisionExit2D() {
        Debug.Log("EXITED A TILE");
        currentLocationCollider = null;
    }

    protected override bool validateDropLocation() {
        // multiple things to check:
        // is the unit even hovering over something?
        // is the space currently occupied by another object?
        if (currentLocationCollider == null) {
            return false;
        }
        return currentLocationCollider != null;
    }

    protected override Vector3 getDropLocationCoords() {
        Vector3 newLocationCoords = currentLocationCollider.transform.position;
        newLocationCoords.z = -1; // fix the z value 
        return newLocationCoords;
    }
}
