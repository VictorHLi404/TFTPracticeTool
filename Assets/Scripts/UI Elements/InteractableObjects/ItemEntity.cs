using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A class that represents an item entity in game. Can be combined with other items (destroys itself and creates a new one on the spot),
/// or can be placed on a champion (destroys itself and champion takes its image)
/// </summary>
public class ItemEntity : DragAndDrop
{
    public GameObject itemIcon;
    public Item item;

    public GameObject currentCollisionObject = null;
    public GameObject previousCollisionObject = null;

    public new void Awake()
    {
        base.Awake();
    }

    public void Initialize(Item item, GameObject itemSlot)
    {
        this.item = item;
        itemIcon = transform.Find("ItemIcon").gameObject;
        SpriteRenderer spriteRendererComponent = itemIcon.GetComponent<SpriteRenderer>();
        string itemName = item.getImageString();
        string file_path = $"ItemIcons/{itemName}";
        spriteRendererComponent.sprite = Resources.Load<Sprite>(file_path);

        this.currentCollisionObject = itemSlot;
        this.previousCollisionObject = itemSlot;

        UpdatePickupCoords(previousCollisionObject.transform.position);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if (isChampion(collisionGameObject))
        {
            currentCollisionObject = collisionGameObject;
        }
        else if (isItem(collisionGameObject))
        {
            currentCollisionObject = collisionGameObject;
        }
        else if (isItemSlot(collisionGameObject))
        {
            currentCollisionObject = collisionGameObject;
        }
    }

    public void OnCollisionExit2D(Collision2D collisionObject)
    {
        GameObject collisionGameObject = collisionObject.gameObject;
        if (isItemSlot(collisionGameObject) && currentCollisionObject == collisionGameObject)
        {
            currentCollisionObject = null;
        }
    }

    public bool isItemSlot(GameObject collisionObject)
    {
        return collisionObject.GetComponent<ItemSlot>() != null;
    }

    public bool isChampion(GameObject collisionObject)
    {
        return collisionObject.GetComponent<ChampionEntity>() != null;
    }

    public bool isItem(GameObject collisionObject)
    {
        return collisionObject.GetComponent<ItemEntity>() != null;
    }

    protected bool ValidateItemSlotDropLocation()
    {
        if (currentCollisionObject == null)
        {
            return false;
        }
        else if (currentCollisionObject.GetComponent<ItemSlot>() == null)
        {
            return false;
        }

        return true;
    }

    protected bool ValidateItemCombinationLocation()
    {
        if (currentCollisionObject == null)
        {
            return false;
        }
        else if (currentCollisionObject.GetComponent<ItemEntity>() == null)
        {
            return false;
        }
        else
        {
            ItemEntity otherItemEntity = currentCollisionObject.GetComponent<ItemEntity>();
            return item.canCombine(otherItemEntity.item);
        }
    }

    protected bool ValidateItemSwapLocation()
    {
        if (currentCollisionObject == null)
        {
            return false;
        }
        else if (currentCollisionObject.GetComponent<ItemEntity>() == null)
        {
            return false;
        }
        return true;
    }

    protected bool ValidateChampionDropLocation()
    {
        if (currentCollisionObject == null || currentCollisionObject.GetComponent<ChampionEntity>() == null)
        {
            return false;
        }
        else if (currentCollisionObject.GetComponent<ChampionEntity>().CanItemBePlaced())
        {
            return false;
        }
        return true;
    }

    protected override void OnMouseUp()
    {
        if (ValidateItemCombinationLocation())
        {
            ItemEntity otherItemEntity = currentCollisionObject.GetComponent<ItemEntity>();
            CompletedItem? newItemEnum = item.combineItem(otherItemEntity.item);
            if (newItemEnum == null)
            {
                Debug.LogError("Something messed up when trying to combine items.");
            }
            Item newItem = new Item(newItemEnum);
            // Assumption is that since this is the item currently being picked up,
            // we want to spawn where the OTHER item is. set things accordingly.

            GameObject newItemEntity = Instantiate(gameObject, transform.parent);
            GameObject newItemSlot = otherItemEntity.previousCollisionObject;
            newItemEntity.GetComponent<ItemEntity>().Initialize(newItem, newItemSlot);
            newItemEntity.transform.localPosition = currentCollisionObject.transform.localPosition;

            previousCollisionObject.GetComponent<ItemSlot>().removeItemFromSlot();
            newItemSlot.GetComponent<ItemSlot>().removeItemFromSlot();

            newItemSlot.GetComponent<ItemSlot>().placeItemInSlot(newItemEntity.GetComponent<ItemEntity>());
            newItemSlot.GetComponent<ItemSlot>().callReshuffle();

            Destroy(otherItemEntity.gameObject);
            Destroy(this.gameObject);
        }
        else if (ValidateItemSwapLocation())
        {
            ItemEntity otherItemEntity = currentCollisionObject.GetComponent<ItemEntity>();
            previousCollisionObject.GetComponent<ItemSlot>().removeItemFromSlot();
            otherItemEntity.previousCollisionObject.GetComponent<ItemSlot>().removeItemFromSlot();
            GameObject tempData = previousCollisionObject;

            previousCollisionObject = otherItemEntity.previousCollisionObject;
            currentCollisionObject = previousCollisionObject;
            Initialize(item, previousCollisionObject);
            previousCollisionObject.GetComponent<ItemSlot>().placeItemInSlot(this);

            otherItemEntity.previousCollisionObject = tempData;
            otherItemEntity.currentCollisionObject = otherItemEntity.previousCollisionObject;
            otherItemEntity.Initialize(otherItemEntity.item, otherItemEntity.previousCollisionObject);
            otherItemEntity.previousCollisionObject.GetComponent<ItemSlot>().placeItemInSlot(otherItemEntity);

            currentCollisionObject.GetComponent<ItemSlot>().callReshuffle();

        }
        else if (ValidateChampionDropLocation())
        {
            previousCollisionObject.GetComponent<ItemSlot>().removeItemFromSlot();
            previousCollisionObject.GetComponent<ItemSlot>().callReshuffle();
            ChampionEntity championEntity = currentCollisionObject.GetComponent<ChampionEntity>();
            championEntity.AddItem(this);
            Destroy(this.gameObject);
        }
        else if (ValidateItemSlotDropLocation())
        {
            // Assumption is that there is NO case in which its possible to place an item in a slot
            // thats already occupied.

            ItemSlot itemSlot = currentCollisionObject.GetComponent<ItemSlot>();
            previousCollisionObject.GetComponent<ItemSlot>().removeItemFromSlot();
            itemSlot.placeItemInSlot(this);
            previousCollisionObject = currentCollisionObject;
            this.transform.position = getDropLocationCoords();
            itemSlot.callReshuffle();
        }

        this.transform.position = pickUpCoords;
    }

    protected override Vector3 getDropLocationCoords()
    {
        Vector3 newLocationCoords = currentCollisionObject.transform.position;
        newLocationCoords.z = -1; // fix the z value 
        return newLocationCoords;
    }

    protected new void UpdatePickupCoords(Vector3 newPositionCoords)
    { // assign the latest position the champion was successfully dropped off at to the variable pickUpCoords
        newPositionCoords.z -= 10;
        pickUpCoords = newPositionCoords;
    }
}
