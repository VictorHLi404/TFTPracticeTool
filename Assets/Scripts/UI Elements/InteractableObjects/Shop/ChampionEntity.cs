using UnityEngine;
using TMPro;

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
    public new void Start()
    {
        base.Start();
        this.border = transform.Find("Border").gameObject;
        this.championNameField = transform.Find("ChampionNameField").gameObject;
        this.championIcon = transform.Find("ChampionIcon").gameObject;
    }

    public void Initialize(Champion newChampion)
    {
        this.champion = newChampion;
        updateVisuals();
    }

    public void updateVisuals()
    {
        championNameField.GetComponent<TextMeshPro>().text = champion.UnitName;
        championIcon.GetComponent<ChampionIcon>().updateChampionImage(champion);
        int starLevel = champion.starLevel;
        SpriteRenderer spriteRenderer = border.GetComponent<SpriteRenderer>();
        if (starLevel == 1) // bronze
        {
            spriteRenderer.color = new Color(0.8f, 0.5f, 0.2f, 1f);
        }
        else if (starLevel == 2) // silver
        {
            spriteRenderer.color = new Color(0.75f, 0.75f, 0.75f, 1f);
        }
        else // gold
        {
            spriteRenderer.color = new Color(1f, 0.84f, 0f, 1f);
        }
    }



}