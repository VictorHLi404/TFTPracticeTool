using UnityEngine;

/// <summary>
/// A class to represent a single champion entity on the board.
/// Changes dynamically to visually represent the current champion it references (e.g show star level, cost, name)
/// Inherits from ChampionInteraction to handle interaction from player.
/// </summary>
public class ChampionEntity : ChampionInteraction
{
    private Champion champion;
    private GameObject border;
    private GameObject championNameField;
    private GameObject championIcon;

    private GameObject itemDisplay;
    public new void Start()
    {
        base.Start();

    }

}