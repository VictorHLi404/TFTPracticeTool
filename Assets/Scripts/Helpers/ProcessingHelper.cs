using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ProcessingHelper
{
    public static string CleanChampionName(string championName)
    {
        return championName.Replace(" ", "");
    }

    public static List<List<AllItemsEnum>> GeneratePossibleItemSets(List<Component> components)
    {
        var indexList = components.Select((x, index) => index).ToList();
        var possibleSets = new List<List<(int firstComponentIndex, int secondComponentIndex)>>();

        GeneratePairingsRecursive(indexList, new List<(int, int)>(), possibleSets);

        var itemSets = new List<List<AllItemsEnum>>();

        var allItemEnumValues = Enum.GetValues(typeof(AllItemsEnum));

        var allItems = new List<AllItemsEnum>();

        foreach (var item in allItemEnumValues)
        {
            allItems.Add((AllItemsEnum)item);
        }

        foreach (var set in possibleSets)
        {
            var itemSet = new List<AllItemsEnum>();
            foreach (var pairing in set)
            {
                Item firstComponent = new Item(components[pairing.firstComponentIndex]);
                Item secondComponent = new Item(components[pairing.secondComponentIndex]);
                var completedItem = firstComponent.combineItem(secondComponent);
                if (completedItem == null)
                    Debug.LogError($"Failed to combine {firstComponent} and {secondComponent}");
                AllItemsEnum finalItem;
                if (!Enum.TryParse(completedItem.ToString(), out finalItem))
                    Debug.LogError($"Failed to convert {completedItem} to the DTOS form AllItemsEnum");
                itemSet.Add(finalItem);
            }
            var isValidList = true;
            foreach (var possibleItem in allItems)
            {
                if (itemSet.Count(item => item == possibleItem) > 2)
                {
                    isValidList = false;
                    break;
                }
            }
            if (isValidList)
                itemSet.Sort();
            itemSets.Add(itemSet);
        }

        var uniqueSets = itemSets.Distinct(new ListComparer<AllItemsEnum>()).ToList();

        return uniqueSets;
    }

    internal class ListComparer<T> : IEqualityComparer<List<T>>
    {
        private readonly IEqualityComparer<T> _elementComparer;

        public ListComparer() : this(EqualityComparer<T>.Default) { }

        public ListComparer(IEqualityComparer<T> elementComparer)
        {
            _elementComparer = elementComparer;
        }

        public bool Equals(List<T>? x, List<T>? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            if (x.Count != y.Count) return false;

            var set1 = new HashSet<T>(x, _elementComparer);
            var set2 = new HashSet<T>(y, _elementComparer);

            return set1.SetEquals(set2);
        }

        public int GetHashCode(List<T> obj)
        {
            int hash = 17;
            foreach (var item in obj.OrderBy(i => i))
            {
                hash = hash * 31 + _elementComparer.GetHashCode(item);
            }
            return hash;
        }
    }

    private static void GeneratePairingsRecursive(
        List<int> remainingElements,
        List<(int, int)> currentPairingSet,
        List<List<(int, int)>> allPairings
    )
    {
        if (remainingElements.Count == 0)
        {
            allPairings.Add(new List<(int, int)>(currentPairingSet));
            return;
        }

        int firstIndex = remainingElements[0];
        for (int i = 1; i < remainingElements.Count; i++)
        {
            int secondIndex = remainingElements[i];
            var newPair = (firstIndex, secondIndex);
            var nextRemainingElements = new List<int>(remainingElements);
            nextRemainingElements.RemoveAt(i);
            nextRemainingElements.RemoveAt(0);

            currentPairingSet.Add(newPair);

            GeneratePairingsRecursive(nextRemainingElements, currentPairingSet, allPairings);

            currentPairingSet.RemoveAt(currentPairingSet.Count - 1);
        }
    }

    public static List<Champion> GetRelaventPossibleChampions(List<(UnitData unit, int occurences)> championOccurences)
    {
        var relaventChampions = new List<Champion>();
        foreach (var championOccurence in championOccurences)
        {
            var starLevel = (int)Math.Log(championOccurence.occurences, 3) + 1;
            var champion = new Champion(starLevel, championOccurence.unit);
            // discard champions that are too low level to meaningfully impact the game
            if (champion.Cost < 3 && champion.starLevel < 2)
            {
                continue;
            }
            relaventChampions.Add(champion);
        }
        return relaventChampions;
    }

    public static (Champion? firstChampion, Champion? secondChampion) GetMostRelaventChampions(List<ChampionEntity> championEntities)
    {
        var sortedChampions = championEntities
            .Select(x => x.champion)
            .OrderByDescending(x => x.GetItems().Count)
            .ThenByDescending(x => x.starLevel * x.Cost)
            .ToList();
        if (sortedChampions.Count < 1)
            return (null, null);
        if (sortedChampions.Count < 2)
            return (sortedChampions[0], null);
        
        return (sortedChampions[0], sortedChampions[1]);
    }
}