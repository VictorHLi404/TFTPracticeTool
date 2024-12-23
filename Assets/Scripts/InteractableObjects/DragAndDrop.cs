using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using Vector3 = UnityEngine.Vector3;

public class DragAndDrop : MonoBehaviour
{

    // all drag and drop objects in tft behave the same way, they NEED to be fixed at a certain location
    // pick up the object and drag as normal, BUT each subclass should be able to validate whether the final drop location is ok or not and
    // decide to either return to their original spot or to move to the new one.
    protected UnityEngine.Vector3 pickUpCoords; // last given position of the object
    protected InputAction mousePosition; // KEEPS TRACK OF WHERE THE MOUSE POSITION IS
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        mousePosition = InputSystem.actions.FindAction("MousePointer"); // access the mouse pointer for input
        UpdatePickupCoords(transform.position); // set the first default return location for the object
    }

    protected void UpdatePickupCoords(UnityEngine.Vector3 newPositionCoords) { // assign the latest position the champion was successfully dropped off at to the variable pickUpCoords
        pickUpCoords = newPositionCoords;
    }


    protected void OnMouseDrag() { // if champion is within a valid grid position, drop it down at that spot, if not, then reset back to previous location
        this.transform.position = getMouseWorldPosition(mousePosition.ReadValue<UnityEngine.Vector2>()); // set the position of the object to the current mouse position
    }

    protected void OnMouseUp() { 
        // check if the location is a valid place for the checkmark to be: if it is, then drop and update new starting, if not, then return to initial place
        if (validateDropLocation()) {
            this.transform.position = getDropLocationCoords();
            pickUpCoords = this.transform.position;
        }
        else {
            this.transform.position = pickUpCoords;
        }
        
    }

    protected UnityEngine.Vector3 getMouseWorldPosition(UnityEngine.Vector2 currentMousePosition) {
        UnityEngine.Vector3 adjustedWorldPosition = Camera.main.ScreenToWorldPoint(currentMousePosition);
        adjustedWorldPosition.z = -1; // some fucked shit because you need to reset the z distance
        return adjustedWorldPosition;
    }

    protected virtual bool validateDropLocation() { // method meant to be overriden, validate drop location DEPENDING on what kind of object
        return false;
    }

    protected virtual Vector3 getDropLocationCoords() {
        return new Vector3(0,0,0);
    }
}
