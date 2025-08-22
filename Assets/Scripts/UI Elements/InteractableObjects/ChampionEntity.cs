using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

/// <summary>
/// A class to represent a single champion entity on the board.
/// Changes dynamically to visually represent the current champion it references (e.g show star level, cost, name)
/// Inherits from ChampionInteraction to handle interaction from player.
/// </summary>
public class ChampionEntity : DragAndDrop
{
    public Champion champion { get; set; }
    public bool isOnBench = true;
    private GameObject border;
    private GameObject championIcon;
    private GameObject itemRenderer;
    private GameObject ShopUIReference;

    public GameObject currentCollisionObject = null; // variable to interface with current hex / bench slot that the unit is sitting on\
    public GameObject previousCollisionObject = null; // variable to keep track of exisiting place, same w drop coords
    private GameObject itemDisplay;
    public new void Awake()
    {
        base.Awake();
        this.border = transform.Find("Border").gameObject;
        this.championIcon = transform.Find("ChampionIcon").gameObject;
        this.itemRenderer = transform.Find("ItemRenderer").gameObject;
    }

    public void Initialize(Champion newChampion, GameObject unitSlot, GameObject ShopUIReference)
    {
        this.champion = newChampion;
        this.currentCollisionObject = unitSlot;
        this.previousCollisionObject = unitSlot;
        this.ShopUIReference = ShopUIReference;
        UpdateVisuals();
        UpdatePickupCoords(previousCollisionObject.transform.position);
    }

    public void UpdateVisuals()
    {
        championIcon.GetComponent<ChampionIcon>().updateChampionImage(champion);
        int starLevel = champion.starLevel;
        SpriteRenderer borderSpriteRenderer = border.GetComponent<SpriteRenderer>();
        if (starLevel == 1) // bronze
        {
            borderSpriteRenderer.color = new Color(0.8f, 0.5f, 0.2f, 1f);
        }
        else if (starLevel == 2) // silver
        {
            borderSpriteRenderer.color = new Color(0.75f, 0.75f, 0.75f, 1f);
        }
        else // gold
        {
            borderSpriteRenderer.color = new Color(1f, 0.84f, 0f, 1f);
        }
        SpriteRenderer championSpriteRenderer = championIcon.GetComponent<SpriteRenderer>();
        championSpriteRenderer.sprite = CropSprite(championSpriteRenderer.sprite);
        UpdateItemVisuals();
    }

    public void UpdateItemVisuals()
    {
        if (itemRenderer == null)
        {
            Debug.LogError("Item renderer for this champion has not been initialized.");
        }
        List<Item> itemList = champion.GetItems();
        itemRenderer.GetComponent<ItemRenderer>().UpdateDisplays(itemList);
    }

    public Sprite CropSprite(Sprite originalSprite)
    {
        Texture2D originalTexture = originalSprite.texture;

        // Create a new sprite from part of the texture
        Sprite croppedSprite = Sprite.Create(
            originalTexture,
        new Rect(
            x: originalTexture.width * 2.5f / 5f, // Start at middle
            y: 0,                 // From bottom
            width: originalTexture.width * 2.5f / 5,
            height: originalTexture.height
        ),
            new Vector2(0.2f, 0.5f), // Pivot
            originalSprite.pixelsPerUnit
        );

        return croppedSprite;
    }

    /// <summary>
    /// Extension call from Champion.
    /// </summary>
    /// <returns></returns>
    public int GetSellPrice()
    {
        return champion.getSellPrice();
    }

    public bool CanItemBePlaced()
    {
        return champion.canItemBePlaced();
    }

    public void AddItem(ItemEntity itemEntity)
    {
        champion.addItem(itemEntity.item);
        UpdateItemVisuals();

    }

    public void LevelUp()
    {
        champion.starLevel++;
        UpdateVisuals();
    }

    public void RemoveSelfFromSlot()
    {
        if (currentCollisionObject == null)
        {
            return;
        }
        if (currentCollisionObject.gameObject.GetComponent<UnitSlot>() == null)
        {
            Debug.LogError("Invalid merging occured when trying to delete old objects");
        }
        UnitSlot championSlot = currentCollisionObject.gameObject.GetComponent<UnitSlot>();
        championSlot.removeChampionFromSlot();
    }

    public void OnCollisionEnter2D(Collision2D collisionObject)
    {
        GameObject collisionGameObject = collisionObject.gameObject;

        if (collisionGameObject.GetComponent<ShopUI>() != null)
        {
            currentCollisionObject = collisionGameObject;
            if (previousCollisionObject == null)
            {
                previousCollisionObject = currentCollisionObject;
            }
        }
        if (isChampion(collisionGameObject))
        {
            currentCollisionObject = collisionGameObject;
            if (previousCollisionObject == null)
            {
                previousCollisionObject = currentCollisionObject;
            }
        }
        else if (isUnitSlot(collisionGameObject))
        {
            if (currentCollisionObject != null && isChampion(currentCollisionObject))
            {
                return;
            }
            currentCollisionObject = collisionGameObject;
            if (previousCollisionObject == null) // handle spawn in case
            {
                previousCollisionObject = currentCollisionObject;
                UpdatePickupCoords(previousCollisionObject.transform.position);
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collisionObject)
    {
        GameObject collisionGameObject = collisionObject.gameObject;
        if (currentCollisionObject == collisionGameObject)
        {
            currentCollisionObject = null;
        }
    }

    /// <summary>
    /// Helper function to check whether collision object is a unit slot or not.
    /// </summary>
    /// <returns></returns>
    protected bool isUnitSlot(GameObject collisionObject)
    {
        return collisionObject.GetComponent<UnitSlot>() != null;
    }

    protected bool isChampion(GameObject collisionObject)
    {
        return collisionObject.GetComponent<ChampionEntity>() != null;
    }


    protected bool validateHexDropLocation()
    {
        // multiple things to check:
        // is the unit even hovering over something?
        // TODO is the space currently occupied by another object? assuming its a hex
        if (currentCollisionObject == null)
        {
            return false;
        }
        else if (currentCollisionObject.GetComponent<UnitSlot>() == null)
        {
            return false;
        }
        else
        {
            UnitManager parentManager = currentCollisionObject.GetComponent<UnitSlot>().parentManager;
            var unitSlot = currentCollisionObject.GetComponent<UnitSlot>();
            var sameParentManager = unitSlot.isBenchSlot == isOnBench;
            if (!parentManager.CanUnitBePlaced(sameParentManager))
            {
                return false;
            }
            return unitSlot.isEmpty();
        }
    }

    protected bool validateShopDropLocation()
    {
        if (currentCollisionObject == null)
        {
            return false;
        }
        else if (currentCollisionObject.GetComponent<ShopUI>() == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected bool validateChampionSwapLocation()
    {
        if (currentCollisionObject == null)
        {
            return false;
        }
        else if (currentCollisionObject.GetComponent<ChampionEntity>() == null)
        {
            return false;
        }
        return true;
    }

    protected void OnMouseOver()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(Settings.Instance.HotkeyBindings[HotkeyEnum.SellChampionHotkey]))
            {
                SellSelf();
            }
        }
    }

    protected override void OnMouseUp()
    {
        // check if the location is a valid place for the checkmark to be: if it is, then drop and update new starting, if not, then return to initial place
        if (validateShopDropLocation())
        {
            currentCollisionObject.GetComponent<ShopUI>().SellChampion(this);
            previousCollisionObject.GetComponent<UnitSlot>().removeChampionFromSlot();
            Destroy(gameObject);
        }
        else if (validateChampionSwapLocation())
        {
            ChampionEntity otherChampion = currentCollisionObject.GetComponent<ChampionEntity>();
            previousCollisionObject.GetComponent<UnitSlot>().removeChampionFromSlot();
            otherChampion.previousCollisionObject.GetComponent<UnitSlot>().removeChampionFromSlot();
            GameObject tempData = previousCollisionObject;

            previousCollisionObject = otherChampion.previousCollisionObject;
            currentCollisionObject = previousCollisionObject;
            UpdatePickupCoords(previousCollisionObject.transform.position);
            previousCollisionObject.GetComponent<UnitSlot>().placeChampionInSlot(this);
            isOnBench = previousCollisionObject.GetComponent<UnitSlot>().isBenchSlot;
            this.transform.position = pickUpCoords;

            otherChampion.previousCollisionObject = tempData;
            otherChampion.currentCollisionObject = otherChampion.previousCollisionObject;
            otherChampion.UpdatePickupCoords(otherChampion.previousCollisionObject.transform.position);
            otherChampion.previousCollisionObject.GetComponent<UnitSlot>().placeChampionInSlot(otherChampion);
            otherChampion.isOnBench = otherChampion.previousCollisionObject.GetComponent<UnitSlot>().isBenchSlot;
            otherChampion.transform.position = otherChampion.pickUpCoords;
        }
        else if (validateHexDropLocation())
        {
            this.transform.position = getDropLocationCoords();
            pickUpCoords = this.transform.position;
            previousCollisionObject.GetComponent<UnitSlot>().removeChampionFromSlot();
            currentCollisionObject.GetComponent<UnitSlot>().placeChampionInSlot(this);
            previousCollisionObject = currentCollisionObject;
            transform.parent = currentCollisionObject.transform.parent;
            isOnBench = currentCollisionObject.GetComponent<UnitSlot>().isBenchSlot;
        }
        else
        {
            this.transform.position = pickUpCoords;
        }
    }

    protected void SellSelf()
    {
        ShopUIReference.GetComponent<ShopUI>().SellChampion(this);
        previousCollisionObject.GetComponent<UnitSlot>().removeChampionFromSlot();
        Destroy(gameObject);
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