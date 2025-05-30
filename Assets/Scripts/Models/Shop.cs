using UnityEngine;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using Unity.VisualScripting;

public class Shop
{

    private List<UnitData> champions; // list of all possible champions
    private Dictionary<int, int> championBagSizes; // dictionary mapping from DATABASE ID to remaining elements within it
    private Dictionary<int, UnitData> championIDtoUnitData;
    private Dictionary<int, List<int>> levelOdds; // mapping of current level to relavent shop odds

    private Player playerData; // data object containing the information of the player
    public List<UnitData> currentShop; // list of current champions available in the shop

    public Shop()
    {
        this.champions = DatabaseAPI.getAllUnitData();
        this.championBagSizes = new Dictionary<int, int>();
        this.championIDtoUnitData = new Dictionary<int, UnitData>();
        foreach (UnitData champion in champions)
        {
            championBagSizes[champion.databaseID] = DatabaseAPI.getBagSize(champion);
            championIDtoUnitData[champion.databaseID] = champion;
        }
        this.levelOdds = DatabaseAPI.getShopOdds();
        this.playerData = new Player(6, 0, 40, 40, 4, 2, DatabaseAPI.getLevelMapping());
    }

    /// <summary>
    ///Given the players current level and if they have enough gold,
    ///generate a shop of 5 characters for the player to see in the currentShop list.
    ///Return true/false depending on if this action is successful.
    /// </summary>
    /// <param name="isStartingShop"> take in a bool related to if this is a starting shop or not, i.e whether to charge the player money</param>
    /// <returns></returns>
    public bool generateShop(bool isStartingShop)
    {
        /*
        Given the players current level and if they have enough gold,
         generate a shop of 5 characters for the player to see in the currentShop list.
        Return true/false depending on if this action is successful.
        */
        if (!isStartingShop)
        {
            if (playerData.gold < 2)
            {
                return false;
            }
            else
            {
                playerData.gold -= 2;
            }
        }

        List<int> currentOdds = levelOdds[playerData.level];
        List<UnitData> newShop = new List<UnitData>();

        // return the champions from the previous shop back into the bags
        if (currentShop != null)
        {
            foreach (UnitData previousShopChampion in currentShop)
            {
                if (!previousShopChampion.isDummy())
                {
                    championBagSizes[previousShopChampion.databaseID] += 1;
                    Debug.Log($"Putting {previousShopChampion} back in the bag...");
                }
            }
        }

        Dictionary<int, int> currentPool = generateCurrentPool();

        for (int i = 0; i < 5; i++)
        { // generate new shop
            System.Random rnd = new System.Random();
            int value = rnd.Next(1, 101);
            int unitCost = 1;

            while (currentOdds[unitCost - 1] < value && unitCost <= 5) // pick the cost level
            { // TODO; PATCH FOR 6 COSTS!
                value -= currentOdds[unitCost - 1];
                unitCost++;
            }

            int unitCostPool = rnd.Next(1, currentPool[unitCost] + 1);
            foreach (KeyValuePair<int, int> championData in championBagSizes) // pick the champion
            {
                int championID = championData.Key;
                int unitCount = championData.Value;
                UnitData champion = championIDtoUnitData[championID];

                if (champion.Cost == unitCost)
                {
                    unitCostPool -= championBagSizes[champion.databaseID];
                    if (unitCostPool <= 0)
                    {
                        championBagSizes[champion.databaseID] -= 1;
                        currentPool[unitCost] -= 1;
                        newShop.Add(champion);
                        break;
                    }
                }
            }
        }
        currentShop = newShop;
        return true;
    }

    /// <summary>
    /// Given the player's current gold, decide if they can buy xp and return true/false if the action succeeds. 
    /// </summary>
    /// <returns>
    /// Whether purchasing XP was sucessful or not.
    /// </returns>
    public bool buyXP()
    {
        Debug.Log("BUY XP!");
        playerData.buyXP(4); // default is 4, change later for potential implements
        return true;
    }

    public int getPlayerLevel()
    {
        return playerData.level;
    }

    /// <summary>
    /// Return the player's current level and XP in a format that can be consumed by the shopUI.
    /// </summary>
    /// <returns>An array containing [current level, current xp, current level XP cap, current gold] </returns>
    public (int level, int xp, int xpCap, int gold, List<int> shopOdds) getDisplayData()
    {
        (int level, int xp, int xpCap, int gold) = playerData.getDisplayData();
        List<int> shopOdds = levelOdds[level];
        return (level, xp, xpCap, gold, shopOdds);
    }

    public bool buyChampion(UnitData champion)
    {
        /*
        Given the unitdata of a champion, evaluate if the player currently has enough money to buy the champion; then, if they do,
        remove them from the current shop pool.
        */
        if (champion.Cost > playerData.gold)
        {
            return false;
        }
        Debug.Log($"Purchasing {champion.UnitName}...");
        playerData.gold -= champion.Cost;
        for (int i = 0; i < currentShop.Count; i++)
        {
            if (champion.DatabaseID == currentShop[i].DatabaseID)
            {
                currentShop.RemoveAt(i);
                return true;
            }
        }
        Debug.LogError("Purchased an impossible champion.");
        return false;
    }

    /// <summary>
    /// Given a Champion object, return the amount its worth to the player and return the amount of units it contains to the pool.
    /// </summary>
    /// <param name="champion"></param>
    /// <returns></returns>

    public bool sellChampion(Champion champion)
    {
        playerData.gold += champion.getSellPrice();
        int unitCount = (int)Math.Pow(3, champion.starLevel - 1);
        championBagSizes[champion.databaseID] += unitCount;
        Debug.Log($"Returned {champion} to the pool. there are now {championBagSizes[champion.databaseID]}  instances.");
        return true;
    }

    private Dictionary<int, int> generateCurrentPool()
    {
        Dictionary<int, int> currentPool = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> championData in championBagSizes)
        {
            int key = championIDtoUnitData[championData.Key].Cost;
            int value = championData.Value;
            if (currentPool.ContainsKey(key))
            {
                currentPool[key] += value;
            }
            else
            {
                currentPool[key] = value;
            }
        }
        return currentPool;
    }



}