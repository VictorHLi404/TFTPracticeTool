using UnityEngine;
using TMPro;
using System;

/// <summary>
/// A class to represent a single champion entity on the board.
/// Changes dynamically to visually represent the current champion it references (e.g show star level, cost, name)
/// Inherits from ChampionInteraction to handle interaction from player.
/// </summary>
public class ChampionEntity : ChampionInteraction
{
    protected Champion champion;
    private GameObject border;
    private GameObject championNameField;
    private GameObject championIcon;

    private GameObject itemDisplay;
    public new void Awake()
    {
        base.Awake();
        this.border = transform.Find("Border").gameObject;
        this.championNameField = transform.Find("ChampionNameField").gameObject;
        this.championIcon = transform.Find("ChampionIcon").gameObject;
    }

    public void Initialize(Champion newChampion)
    {
        this.champion = newChampion;
        updateVisuals();
    }

    public void updateVisuals() // TODO: transform from flat top to pointy top
    {
        championNameField.GetComponent<TextMeshPro>().text = champion.UnitName;
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

}