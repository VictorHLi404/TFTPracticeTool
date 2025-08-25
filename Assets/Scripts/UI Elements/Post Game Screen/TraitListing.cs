using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TraitListing : MonoBehaviour
{
    public List<GameObject> TraitDisplays;
    private TraitTracker traitTracker;
    public readonly int maxTraits = 6;

    public void Initialize(List<Champion> champions)
    {
        traitTracker = new TraitTracker();
        traitTracker.UpdateCurrentTraits(champions);
        var traits = traitTracker.GetSortedTraits();
        traits = traits.Where(x => x.traitCount >= x.traitBreakpoints[0]).Take(maxTraits).ToList();
        for (int i = 0; i < Math.Min(TraitDisplays.Count, traits.Count); i++)
        {
            var trait = traits[i];
            var traitDisplay = TraitDisplays[i].GetComponent<TraitDisplay>();
            traitDisplay.Initialize(trait.traitName, trait.traitCount, trait.traitBreakpoints, trait.traitRarities, true);
        }
        if (traits.Count < TraitDisplays.Count || traits.Count < maxTraits)
        {
            for (int i = traits.Count; i < TraitDisplays.Count; i++)
            {
                TraitDisplays[i].SetActive(false);
            }
        }
    }
}