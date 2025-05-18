using UnityEngine;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using Unity.VisualScripting;

public class Shop
{

    private List<UnitData> champions; // list of all possible champions
    private Dictionary<UnitData, int> championBagSizes; // dictionary mapping from champion name to remaining elements within it
    private Dictionary<int, List<int>> levelOdds; // mapping of current level to relavent shop odds

    private Player playerData; // data object containing the information of the player
    public List<UnitData> currentShop; // list of current champions available in the shop

    public Shop()
    {
        this.champions = DatabaseAPI.getAllUnitData();
        this.championBagSizes = new Dictionary<UnitData, int>();
        foreach (UnitData champion in champions)
        {
            championBagSizes[champion] = DatabaseAPI.getBagSize(champion);
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
                    championBagSizes[previousShopChampion] += 1;
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

            while (currentOdds[unitCost - 1] < value && unitCost <= 5)
            { // TODO; PATCH FOR 6 COSTS!
                value -= currentOdds[unitCost - 1];
                unitCost++;
            }

            int unitCostPool = rnd.Next(1, currentPool[unitCost] + 1);
            foreach (KeyValuePair<UnitData, int> championData in championBagSizes)
            {
                UnitData champion = championData.Key;
                int unitCount = championData.Value;
                if (champion.Cost == unitCost)
                {
                    unitCostPool -= championBagSizes[champion];
                    if (unitCostPool <= 0)
                    {
                        Debug.Log($"There are currently {championBagSizes[champion]} {champion.UnitName} in play.");
                        championBagSizes[champion] -= 1;
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

    /// <summary>
    /// Return the player's current level and XP in a format that can be consumed by the shopUI.
    /// </summary>
    /// <returns>An array containing [current level, current xp, current level XP cap, current gold] </returns>
    public (int level, int xp, int xpCap, int gold) getDisplayData()
    {
        return playerData.getDisplayData();
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

    private Dictionary<int, int> generateCurrentPool()
    {
        Dictionary<int, int> currentPool = new Dictionary<int, int>();
        foreach (KeyValuePair<UnitData, int> championData in championBagSizes)
        {
            int key = championData.Key.Cost;
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