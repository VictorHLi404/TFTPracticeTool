using System.Collections.Generic;
using System.Linq;

public class TraitTracker
{
    private int currentChampionCount = 0;
    private Dictionary<string, int> traitCountMapping = new Dictionary<string, int>();
    private Dictionary<string, (List<int>, List<TraitRarities>)> traitDataMapping = new Dictionary<string, (List<int>, List<TraitRarities>)>();

    public TraitTracker()
    {
        this.traitDataMapping = DatabaseAPI.getTraits();
    }

    public int GetCurrentChampionCount()
    {
        return currentChampionCount;
    }

    public void UpdateCurrentTraits(List<Champion> champions)
    {
        Dictionary<string, int> newTraitMapping = new Dictionary<string, int>();
        HashSet<int> championHashSet = new HashSet<int>();

        foreach (Champion championData in champions)
        {
            if (!championHashSet.Contains(championData.DatabaseID))
            {
                championHashSet.Add(championData.DatabaseID);
                List<string> traits = championData.UnitTraits;
                foreach (string trait in traits)
                {
                    if (newTraitMapping.ContainsKey(trait))
                    {
                        newTraitMapping[trait] += 1;
                    }
                    else
                    {
                        newTraitMapping[trait] = 1;
                    }
                }
            }
        }
        traitCountMapping = newTraitMapping;
    }
    
    public List<(string traitName, int traitCount, List<int> traitBreakpoints, List<TraitRarities> traitRarities)> GetSortedTraits()
    {
        var isActiveTraits = new List<(string traitName, int traitCount, int closestBreakpoint)>();
        var isInactiveTraits = new List<(string traitName, int traitCount, int closestBreakpoint)>();

        foreach (KeyValuePair<string, int> trait in traitCountMapping)
        {
            List<int> traitBreakpoints = traitDataMapping[trait.Key].Item1;
            if (trait.Value < traitBreakpoints[0])
            {
                isInactiveTraits.Add((trait.Key, trait.Value, traitBreakpoints[0]));
            }
            else
            {
                int index = 0;
                while (index < traitBreakpoints.Count-1 && trait.Value >= traitBreakpoints[index + 1])
                {
                    index += 1;
                }
                isActiveTraits.Add((trait.Key, trait.Value, index));
            }
        }

        var sortedActiveTraits = isActiveTraits.OrderByDescending(s => s.Item3).ThenBy(s => s.Item1).ToList();
        var sortedInactiveTraits = isInactiveTraits.OrderByDescending(s => (float) s.Item2 / s.Item3).ThenBy(s => s.Item1).ToList();

        var returnList = new List<(string traitName, int traitCount, List<int> traitBreakpoints, List<TraitRarities> traitRarities)>();
        foreach ((string, int, int) trait in sortedActiveTraits)
        {
            returnList.Add((trait.Item1, trait.Item2, traitDataMapping[trait.Item1].Item1, traitDataMapping[trait.Item1].Item2));
        }
        foreach ((string, int, int) trait in sortedInactiveTraits)
        {
            returnList.Add((trait.Item1, trait.Item2, traitDataMapping[trait.Item1].Item1, traitDataMapping[trait.Item1].Item2));
        }
        return returnList;
    }
}