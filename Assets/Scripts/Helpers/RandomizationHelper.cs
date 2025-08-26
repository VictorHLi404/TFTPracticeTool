using System;
using System.Collections.Generic;
using System.Linq;
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
            int itemCountToTake = UnityEngine.Random.Range(4, 5);
            for (int j = 1; j <= itemCountToTake; j++)
            {
                var itemIndexToTake = UnityEngine.Random.Range(0, initialPool.Count);
                selectedComponents.Add(initialPool[itemIndexToTake]);
                initialPool.RemoveAt(itemIndexToTake);
            }
        }
        if (selectedComponents.Count % 2 != 0)
        {
            var itemIndexToTake = UnityEngine.Random.Range(0, initialPool.Count);
            selectedComponents.Add(initialPool[itemIndexToTake]);
            initialPool.RemoveAt(itemIndexToTake);
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

    public static List<Champion> DelevelTeam(List<Champion> champions)
    {
        foreach (var champion in champions)
        {
            if (champion.starLevel == 5)
            {
                champion.starLevel = 1;
            }
            else if (champion.starLevel == 2 && champion.Cost >= 4)
            {
                bool delevel = UnityEngine.Random.Range(0, 100) <= 95;
                if (delevel)
                    champion.starLevel = 1;
            }
            else if (champion.starLevel == 2 && champion.Cost >= 3)
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
        champions = champions.OrderByDescending(x => x.Cost * x.starLevel).ToList();
        champions.RemoveAt(0);
        return champions;
    }
}