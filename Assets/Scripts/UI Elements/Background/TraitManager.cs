using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// A class responsible for scanning and displaying the current active traits on the board.
/// Contains a reference to the traitdisplayer class, which handles backend calculations.
/// Generates trait display objects in an appropriate list.
/// </summary>
public class TraitManager : MonoBehaviour
{

    [Header("Object References")]
    public GameObject board;
    private int currentChampionCount = 0;
    private Dictionary<string, int> traitCountMapping = new Dictionary<string, int>();

    private Dictionary<string, (List<int>, List<TFTEnums.TraitRarities>)> traitDataMapping = new Dictionary<string, (List<int>, List<TFTEnums.TraitRarities>)>();


    public void Start()
    {
        this.traitDataMapping = DatabaseAPI.getTraits();
        foreach (KeyValuePair<string, (List<int>, List<TFTEnums.TraitRarities>)> kvp in traitDataMapping)
        {
            Debug.Log(kvp.Key);
            foreach (int level in kvp.Value.Item1)
            {
                Debug.Log(level);
            }
            foreach (TFTEnums.TraitRarities rarity in kvp.Value.Item2)
            {
                Debug.Log(rarity);
            }
        }   
    }

    public void Update()
    {
        if (board.GetComponent<HexGridManager>() != null)
        {
            if (currentChampionCount != board.GetComponent<HexGridManager>().GetChampionEntities().Count)
            {
                Debug.Log("SHITS GOING DOWN!!!");
                List<ChampionEntity> championEntities = board.GetComponent<HexGridManager>().GetChampionEntities();
                currentChampionCount = championEntities.Count;
                updateCurrentTraits(championEntities);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="championEntities"></param>
    /// <returns></returns>
    private void updateCurrentTraits(List<ChampionEntity> championEntities)
    {
        Dictionary<string, int> newTraitMapping = new Dictionary<string, int>();
        HashSet<int> championHashSet = new HashSet<int>();
        Debug.Log(championEntities.Count);

        foreach (ChampionEntity championEntity in championEntities)
        {
            Champion championData = championEntity.champion;
            if (!championHashSet.Contains(championData.databaseID))
            {
                championHashSet.Add(championData.databaseID);
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

        foreach (KeyValuePair<string, int> kvp in newTraitMapping)
        {
            Debug.Log($"{kvp.Key}, {kvp.Value}");
        }

        traitCountMapping = newTraitMapping;
    }


}