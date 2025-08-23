using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


/// <summary>
/// A class responsible for scanning and displaying the current active traits on the board.
/// Contains a reference to the traitdisplayer class, which handles backend calculations.
/// Generates trait display objects in an appropriate list.
/// </summary>
public class TraitManager : MonoBehaviour
{

    [Header("Object References")]
    public GameObject board;
    public GameObject traitTemplate;
    private TraitTracker traitTracker;

    public void Awake()
    {
        this.traitTracker = new TraitTracker();
    }

    public void Update()
    {
        if (board.GetComponent<HexGridManager>() != null)
        {
            if (traitTracker.GetCurrentChampionCount() != board.GetComponent<HexGridManager>().GetChampionEntities().Count)
            {
                List<ChampionEntity> championEntities = board.GetComponent<HexGridManager>().GetChampionEntities();
                // currentChampionCount = championEntities.Count;
                // UpdateCurrentTraits(championEntities);
                var champions = championEntities.Select(x => x.champion).ToList();
                traitTracker.UpdateCurrentTraits(champions);
                CreateTraitDisplays();
            }
        }
    }

    /// <summary>
    /// Destroys all previous existing trait displays, and regenerates new ones based off of traitCountMapping.
    /// </summary>
    private void CreateTraitDisplays()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject childObject = transform.GetChild(i).gameObject;
                if (childObject.GetComponent<TraitDisplay>() != null)
                {
                    Destroy(childObject);
                }
                else
                {
                    Debug.LogError("A non-trait display child is a child of the trait manager object.");
                }
            }
        }

        float yPosition = 0f;
        var sortedTraits = traitTracker.GetSortedTraits();
        int maxTraits = 7;

        for (int i = 0; i < Math.Min(sortedTraits.Count, maxTraits); i++)
        {
            var traitData = sortedTraits[i];
            GameObject newTraitDisplay = Instantiate(traitTemplate, transform);
            newTraitDisplay.transform.localPosition = new Vector3(0, yPosition, 0);
            newTraitDisplay.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            newTraitDisplay.GetComponent<TraitDisplay>().Initialize(traitData.traitName, traitData.traitCount, traitData.traitBreakpoints, traitData.traitRarities);
            yPosition -= 0.12f;
        }
    }
}