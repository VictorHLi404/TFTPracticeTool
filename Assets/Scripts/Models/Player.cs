using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using UnityEngine.Rendering;

public class Player
{

    public int level { get; set; } = 1;

    public int xp { get; set; } = 0;
    public int gold { get; set; } = 50;
    public float time { get; set; } = 40;
    public int stage { get; set; } = 1;
    public int round { get; set; } = 1;
    private Dictionary<int, int> levelMapping { get; set; } = new Dictionary<int, int>();
    public Player(int level, int xp, int gold, float time, int stage, int round, Dictionary<int, int> levelMapping)
    {
        this.level = level;
        this.xp = xp;
        this.gold = gold;
        this.time = time;
        this.stage = stage;
        this.round = round;
        this.levelMapping = levelMapping;
    }

    // Methods
    public void UpdateGold(int amount)
    {
        this.gold += amount;
    }

    public void AdvanceStage()
    {
        this.stage++;
        this.round = 1; // Reset round to 1 every time stage is increased
    }

    /// <summary>
    /// Increase your current XP. If you level up, adjust the level and new xp goal accordingly. 
    /// </summary>
    /// <param name="xpAmount">
    /// The amount of 
    /// </param>
    public void buyXP(int xpAmount)
    {
        if (gold < xpAmount)
        {
            return;
        }
        if (level > 9)
        { // maxed out
            return;
        }
        gold -= xpAmount;
        xp += xpAmount;
        int levelGoal = levelMapping[level + 1];
        if (xp >= levelGoal)
        {
            xp -= levelGoal;
            level += 1;
        }
    }

    /// <summary>
    /// Return the information related to levels in a consumable format for display.
    /// </summary>
    /// <returns>
    /// A tuple containing the (level, xp, xpCap).
    /// </returns>
    public (int level, int xp, int xpCap) getLevelData()
    {
        if (level > 9)
        {   // maxed out
            return (10, 0, 0);
        }
        else
        {
            return (level, xp, levelMapping[level + 1]);
        }
    }

}