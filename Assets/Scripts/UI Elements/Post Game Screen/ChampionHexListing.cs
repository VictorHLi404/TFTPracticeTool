using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using UnityEngine;

public class ChampionHexListing : MonoBehaviour
{
    [Header("Reference Fields")]
    public List<GameObject> ChampionHexReferences;

    public void Initialize(List<Champion> champions)
    {
        int index = 0;
        var sortedChampions = SortChampionsForDisplay(champions);
        foreach (var champion in sortedChampions)
        {
            var championHex = ChampionHexReferences[index].GetComponent<ChampionSmallInformationDisplay>();
            championHex.UpdateVisuals(champion, true);
            index++;
        }
        if (index < ChampionHexReferences.Count)
        {
            for (int i = index; i < ChampionHexReferences.Count; i++)
            {
                ChampionHexReferences[i].SetActive(false);
            }
        }
    }

    public List<Champion> SortChampionsForDisplay(List<Champion> champions)
    {
        var sortedChampions = champions.OrderByDescending(x => x.Cost)
                                    .ThenByDescending(x => x.starLevel)
                                    .ToList();
        return sortedChampions;
    }
    
}