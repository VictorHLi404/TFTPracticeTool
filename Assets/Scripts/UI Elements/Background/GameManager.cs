using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A class that handles total game operations, primarily moving and combining units
/// </summary>
public class GameManager : MonoBehaviour
{
    private GameObject boardManager;
    private GameObject benchManager;
    private Dictionary<string, int> traitLevels;
    private Dictionary<(int, int), int> currentChampions; // (databaseID, starLevel): count

    public void Awake()
    {
        this.boardManager = transform.Find("BoardManager").gameObject;
        this.benchManager = transform.Find("BenchSlotManager").gameObject;
        if (!boardManager || !benchManager)
        {
            Debug.LogError("Game manager children not assigned properly.");
        }
        this.currentChampions = new Dictionary<(int, int), int>();
    }

    /// <summary>
    /// go through all of the champions, and see if there are any triplets of the SAME champion of the SAME star level.
    /// If they are, delete all three and create a new instance. 
    /// </summary>
    public void Update()
    {

        Dictionary<(int, int), int> newCurrentChampions = new Dictionary<(int, int), int>();
        List<ChampionEntity> championList = new List<ChampionEntity>();

        List<ChampionEntity> benchChampions = benchManager.GetComponent<BenchManager>().GetChampionEntities();
        List<ChampionEntity> boardChampions = boardManager.GetComponent<HexGridManager>().GetChampionEntities();

        benchChampions.AddRange(boardChampions);

        foreach (ChampionEntity championEntity in benchChampions)
        {
            int databaseID = championEntity.champion.databaseID;
            int starLevel = championEntity.champion.starLevel;
            if (!newCurrentChampions.ContainsKey((databaseID, starLevel)))
            {
                newCurrentChampions[(databaseID, starLevel)] = 1;
            }
            else
            {
                newCurrentChampions[(databaseID, starLevel)] += 1;
            }
            championList.Add(championEntity);
        }

        foreach (KeyValuePair<(int, int), int> kvp in newCurrentChampions)
        {
            int databaseID = kvp.Key.Item1;
            int starLevel = kvp.Key.Item2;
            int unitCount = kvp.Value;
            if (unitCount >= 3)
            {
                List<ChampionEntity> championsToMerge = new List<ChampionEntity>();
                foreach (ChampionEntity potentialChampion in championList)
                {
                    if (potentialChampion.champion.databaseID == databaseID && potentialChampion.champion.starLevel == starLevel)
                    {
                        championsToMerge.Add(potentialChampion);
                    }
                }
                mergeChampions(championsToMerge);
            }
        }
        currentChampions = newCurrentChampions;
    }


    /// <summary>
    /// Given a list of champions, go through them and determine:
    /// 1. Which champion should serve as the basis for the new spawn location
    /// 2. How to merge the items (TODO)
    /// Then, create a new instance of the champion with the new items and at the determined location
    /// </summary>
    /// <param name="champions"></param>
    /// <returns></returns>
    public void mergeChampions(List<ChampionEntity> champions)
    {
        ChampionEntity championToKeep = null;
        int index = 0;
        for (int i = 0; i < champions.Count; i++)
        {
            if (!champions[i].isOnBench)
            {
                championToKeep = champions[i];
                index = i;
            }
        }
        if (!championToKeep)
        {
            championToKeep = champions[0];
        }
        championToKeep.LevelUp();
        for (int i = 0; i < champions.Count; i++)
        {
            if (index != i)
            {
                ChampionEntity championEntity = champions[i];
                championEntity.RemoveSelfFromSlot();
                Destroy(champions[i].gameObject);
            }
        }
    }

}