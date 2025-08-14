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

    private int currentChampionCount = 0;
    private Dictionary<string, int> traitCountMapping = new Dictionary<string, int>();

    private Dictionary<string, (List<int>, List<TraitRarities>)> traitDataMapping = new Dictionary<string, (List<int>, List<TraitRarities>)>();


    public void Awake()
    {
        this.traitDataMapping = DatabaseAPI.getTraits();
    }

    public void Update()
    {
        if (board.GetComponent<HexGridManager>() != null)
        {
            if (currentChampionCount != board.GetComponent<HexGridManager>().GetChampionEntities().Count)
            {
                List<ChampionEntity> championEntities = board.GetComponent<HexGridManager>().GetChampionEntities();
                currentChampionCount = championEntities.Count;
                updateCurrentTraits(championEntities);
            }
        }
    }

    /// <summary>
    /// A function that, given a list of entities representing the champions currently on the board,
    /// extracts the relevant trait data and assigns it to the storage. Then calls createTraitDisplays()
    /// to build the new traits.
    /// </summary>
    /// <param name="championEntities"></param>
    /// <returns></returns>
    private void updateCurrentTraits(List<ChampionEntity> championEntities)
    {
        Dictionary<string, int> newTraitMapping = new Dictionary<string, int>();
        HashSet<int> championHashSet = new HashSet<int>();

        foreach (ChampionEntity championEntity in championEntities)
        {
            Champion championData = championEntity.champion;
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
        CreateTraitDisplays();
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
        float yPosition = 0;

        List<(string, int)> sortedTraits = GetSortedTraits();

        var maxTraits = 7;

        for (int i = 0; i < Math.Min(sortedTraits.Count, maxTraits); i++)
        {
            (string, int) traitData = sortedTraits[i];
            GameObject newTraitDisplay = Instantiate(traitTemplate, transform);
            newTraitDisplay.transform.localPosition = new Vector3(0, yPosition, 0);
            newTraitDisplay.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            newTraitDisplay.GetComponent<TraitDisplay>().Initialize(traitData.Item1, traitData.Item2, traitDataMapping[traitData.Item1].Item1, traitDataMapping[traitData.Item1].Item2);

            yPosition -= 0.12f;
        }

        // foreach ((string, int) traitData in sortedTraits)
        // {
        //     if (index >= maxTraits)
        //         break;
        //     GameObject newTraitDisplay = Instantiate(traitTemplate, transform);
        //     newTraitDisplay.transform.localPosition = new Vector3(0, yPosition, 0);
        //     newTraitDisplay.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        //     newTraitDisplay.GetComponent<TraitDisplay>().Initialize(traitData.Item1, traitData.Item2, traitDataMapping[traitData.Item1].Item1, traitDataMapping[traitData.Item1].Item2);

        //     yPosition -= 0.12f;
        // }
    }


    /// <summary>
    /// A function that returns a sorted list of traits in an optimal display order. Prioritizes by:
    /// Is completed / active -> tier of trait completeness -> alphabetical
    /// Tier of trait completeness
    /// 
    /// </summary>
    /// <returns></returns>
    private List<(string, int)> GetSortedTraits()
    {
        List<(string, int, int)> isActiveTraits = new List<(string, int, int)>();
        List<(string, int, int)> isInactiveTraits = new List<(string, int, int)>();

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

        List<(string, int, int)> sortedActiveTraits = isActiveTraits.OrderByDescending(s => s.Item3).ThenBy(s => s.Item1).ToList();
        List<(string, int, int)> sortedInactiveTraits = isInactiveTraits.OrderByDescending(s => (float) s.Item2 / s.Item3).ThenBy(s => s.Item1).ToList();

        List<(string, int)> returnList = new List<(string, int)>();
        foreach ((string, int, int) trait in sortedActiveTraits)
        {
            returnList.Add((trait.Item1, trait.Item2));
        }
        foreach ((string, int, int) trait in sortedInactiveTraits)
        {
            returnList.Add((trait.Item1, trait.Item2));
        }
        return returnList;
    }
}