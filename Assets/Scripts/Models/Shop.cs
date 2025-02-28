using UnityEngine;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using Unity.VisualScripting;

public class Shop {

    private List<UnitData> champions; // list of all possible champions
    private Dictionary<UnitData, int> championBagSizes; // dictionary mapping from champion name to remaining elements within it
    private Dictionary<int, List<int>> levelOdds; // mapping of current level to relavent shop odds
    public List<UnitData> currentShop; // list of current champions available in the shop

    public Shop() {
        this.champions = DatabaseAPI.getAllUnitData();
        this.championBagSizes = new Dictionary<UnitData, int>();
        foreach (UnitData champion in champions) {
            championBagSizes[champion] = DatabaseAPI.getBagSize(champion);
        }
        this.levelOdds = DatabaseAPI.getShopOdds();
    }

    public void generateShop() {
        /*
        Given the players current level, generate a shop of 5 characters for the player to see.
        */
        List<int> currentOdds = levelOdds[7];
        List<UnitData> newShop = new List<UnitData>();

        // return the champions from the previous shop back into the bags
        if (currentShop != null) {
            foreach (UnitData previousShopChampion in currentShop) {
                if (!previousShopChampion.isDummy()) {
                    championBagSizes[previousShopChampion] += 1;
                }
            }
        }

        Dictionary<int, int> currentPool = generateCurrentPool();

        for (int i = 0; i < 5; i++) { // generate new shop
            System.Random rnd = new System.Random();
            int value = rnd.Next(1, 101);
            int unitCost = 1;

            while (currentOdds[unitCost-1] < value && unitCost <= 5) { // TODO; PATCH FOR 6 COSTS!
                value -= currentOdds[unitCost-1];
                unitCost++;
            }

            int unitCostPool = rnd.Next(1, currentPool[unitCost]+1);
            foreach (KeyValuePair<UnitData, int> championData in championBagSizes) {
                UnitData champion = championData.Key;
                int unitCount = championData.Value;
                if (champion.Cost == unitCost) {
                    unitCostPool -= championBagSizes[champion];
                    if (unitCostPool <= 0) {
                        championBagSizes[champion] -= 1;
                        currentPool[unitCost] -= 1;
                        newShop.Add(champion);
                        break;
                    }
                }
             }
        }
        currentShop = newShop;
    }

    private Dictionary<int, int> generateCurrentPool() {
        Dictionary<int, int> currentPool = new Dictionary<int, int>();
        foreach (KeyValuePair<UnitData, int> championData in championBagSizes) {
            int key = championData.Key.Cost;
            int value = championData.Value;
            if (currentPool.ContainsKey(key)) {
                currentPool[key] += value;
            }
            else {
                currentPool[key] = value;
            }
        }
        return currentPool;
    }



}