using System;
using System.Collections.Generic;
using UnityEngine;
public class RandomizationHelper
{

    public static readonly int itemRounds = 3;

    public static List<Component> GenerateRandomComponents()
    {
        var selectedComponents = new List<Component>();
        var initialPool = new List<Component>();
        for (int i = 1; i <= itemRounds; i++)
        {
            AddItemsToPool(initialPool);
            int itemCountToTake = UnityEngine.Random.Range(5, 6);
            for (int j = 1; j <= itemCountToTake; j++)
            {
                var itemIndexToTake = UnityEngine.Random.Range(0, initialPool.Count);
                selectedComponents.Add(initialPool[itemIndexToTake]);
                initialPool.RemoveAt(itemIndexToTake);
            }
        }
        return selectedComponents;
    }

    private static void AddItemsToPool(List<Component> pool)
    {
        foreach (var item in Enum.GetValues(typeof(Component)))
        {
            pool.Add((Component)item);
        }
    }

    public static void DelevelTeam(List<Champion> champions)
    {
        foreach (var champion in champions)
        {
            if (champion.starLevel == 2 && champion.Cost >= 3)
            {
                bool delevel = UnityEngine.Random.Range(0, 100) <= 40;
                if (delevel)
                    champion.starLevel = 1;
            }
            else if (champion.starLevel == 3)
            {
                champion.starLevel -= 1;
            }
        }
    }
}