using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

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

    public GameObject currentCollisionObject = null; // variable to interface with current hex / bench slot that the unit is sitting on\
    public GameObject previousCollisionObject = null; // variable to keep track of exisiting place, same w drop coords
    private GameObject itemDisplay;
    public new void Awake()
    {
        base.Awake();
        this.border = transform.Find("Border").gameObject;
        this.championIcon = transform.Find("ChampionIcon").gameObject;
    }

    public void Initialize(Champion newChampion, GameObject unitSlot)
    {
        this.champion = newChampion;
        this.currentCollisionObject = unitSlot;
        this.previousCollisionObject = unitSlot;
        updateVisuals();
    }

    /// <summary>
    /// Helper function to check whether collision object is a unit slot or not.
    /// </summary>
    /// <returns></returns>
    protected bool isUnitSlot(GameObject collisionObject)
    {
        UnitSlot checkSlot = collisionObject.GetComponent<UnitSlot>();
        return checkSlot != null;
    }

    public void updateVisuals() 
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
    }

    public Sprite CropSprite(Sprite originalSprite)
    {
        Texture2D originalTexture = originalSprite.texture;

        // Create a new sprite from part of the texture
        Sprite croppedSprite = Sprite.Create(
            originalTexture,
        new Rect(
            x: originalTexture.width * 2 / 5, // Start at middle
            y: 0,                 // From bottom
            width: originalTexture.width * 3 / 5,
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
    public int getSellPrice()
    {
        return champion.getSellPrice();
    }

    public void LevelUp()
    {
        champion.starLevel++;
        updateVisuals();
    }

    public void removeSelfFromSlot()
    {
        if (currentCollisionObject == null)
        {
            Debug.Log("CHAMPION DIDNT TOUCH ANYTHING");
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
        if (isUnitSlot(collisionGameObject))
        {
            currentCollisionObject = collisionGameObject;
            if (previousCollisionObject == null) // handle spawn in case
            {
                previousCollisionObject = currentCollisionObject;
                UpdatePickupCoords(previousCollisionObject.transform.position);
            }
        }
        else if (collisionGameObject.GetComponent<ShopUI>() != null)
        {
            currentCollisionObject = collisionGameObject;
            if (previousCollisionObject == null)
            {
                previousCollisionObject = currentCollisionObject;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collisionObject)
    {
        GameObject collisionGameObject = collisionObject.gameObject;
        if (isUnitSlot(collisionGameObject))
        {
            currentCollisionObject = null;
        }
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
            if (!parentManager.CanUnitBePlaced())
            {
                return false;
            }
            return currentCollisionObject.GetComponent<UnitSlot>().isEmpty();
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

    protected override void OnMouseUp()
    {
        // check if the location is a valid place for the checkmark to be: if it is, then drop and update new starting, if not, then return to initial place
        if (validateHexDropLocation())
        {
            this.transform.position = getDropLocationCoords();
            pickUpCoords = this.transform.position;
            previousCollisionObject.GetComponent<UnitSlot>().removeChampionFromSlot();
            currentCollisionObject.GetComponent<UnitSlot>().placeChampionInSlot(this);
            previousCollisionObject = currentCollisionObject;
            transform.parent = currentCollisionObject.transform.parent;
            isOnBench = currentCollisionObject.GetComponent<UnitSlot>().isBenchSlot;
        }
        else if (validateShopDropLocation())
        {
            currentCollisionObject.GetComponent<ShopUI>().sellChampion(this);
            previousCollisionObject.GetComponent<UnitSlot>().removeChampionFromSlot();
            Destroy(gameObject);
        }
        else
        {
            this.transform.position = pickUpCoords;
        }

    }
    protected override Vector3 getDropLocationCoords()
    {
        Vector3 newLocationCoords = currentCollisionObject.transform.position;
        newLocationCoords.z = -1; // fix the z value 
        return newLocationCoords;
    }
}