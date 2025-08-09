using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PostGameModal : MonoBehaviour
{
    private bool isLoading;

    [Header("Reference Fields")]
    public GameObject FirstChampion;
    public GameObject SecondChampion;
    public GameObject Team;

    private bool requestsHaveBeenMade;

    public async UniTask Initialize(List<ChampionEntity> teamChampions,
                            List<(UnitData unit, int occurences)> shopChampions,
                            List<Component> initialComponents)
    {
        var relaventChampions = ProcessingHelper.GetRelaventPossibleChampions(shopChampions);
        var possibleItemSets = ProcessingHelper.GeneratePossibleItemSets(initialComponents);
        var team = new List<Champion>();
        foreach (var championEntity in teamChampions)
        {
            team.Add(championEntity.champion);
        }
        (var firstChampion, var secondChampion) = ProcessingHelper.GetMostRelaventChampions(teamChampions);
        var firstChampionWinrate = await ApiClient.GetChampionWinrate(firstChampion);
        var secondChampionWinrate = await ApiClient.GetChampionWinrate(secondChampion);
        var firstChampionAlternativeBuilds = await ApiClient.GetChampionAlternativeBuilds(firstChampion, possibleItemSets);
        var secondChampionAlternativeBuilds = await ApiClient.GetChampionAlternativeBuilds(secondChampion, possibleItemSets);
        var teamWinrate = await ApiClient.GetTeamWinrate(team);
        var alternativeTeamComps = await ApiClient.GetTeamAlternativeComps(team, relaventChampions);

        Debug.Log($"POSSIBLE ITEM SETS: {possibleItemSets.Count}");
        Debug.Log($"RELAVENT CHAMPIONS: {relaventChampions.Count}");
        Debug.Log($"ALTERNATIVE TEAM COMPS LISTING: {alternativeTeamComps.Count}");
        Debug.Log("GOT HERE!");
    }


}