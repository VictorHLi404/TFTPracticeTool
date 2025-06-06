using Unity.VisualScripting;
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
        Debug.Log(file_path);
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
            Debug.Log("ENTERING A CHAMPION");
        }
        else if (isItem(collisionGameObject))
        {
            currentCollisionObject = collisionGameObject;
            Debug.Log("ENTERING AN ITEM!");
        }
        else if (isItemSlot(collisionGameObject))
        {
            currentCollisionObject = collisionGameObject;
            Debug.Log("ENTERING AN ITEM SLOT");
        }
    }

    public void OnCollisionExit2D(Collision2D collisionObject)
    {
        GameObject collisionGameObject = collisionObject.gameObject;
        if (isItemSlot(collisionGameObject))
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

    protected bool validateItemSlotDropLocation()
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

    protected bool validateItemCombinationLocation()
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
            Debug.Log("CHECK THIS SHIT!");
            ItemEntity otherItemEntity = currentCollisionObject.GetComponent<ItemEntity>();
            return item.canCombine(otherItemEntity.item);
        }
    }

    protected bool validateChampionDropLocation()
    {
        return false;
    }

    protected override void OnMouseUp()
    {
        Debug.Log(pickUpCoords);
        if (validateItemCombinationLocation())
        {
            ItemEntity otherItemEntity = currentCollisionObject.GetComponent<ItemEntity>();
            TFTEnums.Item? newItemEnum = item.combineItem(otherItemEntity.item);

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

            Destroy(currentCollisionObject);
            Destroy(this.gameObject);

        }
        else if (validateChampionDropLocation())
        {

        }
        else if (validateItemSlotDropLocation())
        {

        }
        this.transform.position = pickUpCoords;
    }

    protected override Vector3 getDropLocationCoords()
    {
        Vector3 newLocationCoords = currentCollisionObject.transform.position;
        newLocationCoords.z = -1; // fix the z value 
        return newLocationCoords;
    }

    protected new void UpdatePickupCoords(UnityEngine.Vector3 newPositionCoords)
    { // assign the latest position the champion was successfully dropped off at to the variable pickUpCoords
        newPositionCoords.z -= 1;
        pickUpCoords = newPositionCoords;
    }
}
