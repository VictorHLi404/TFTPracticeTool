using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PostGameModal : MonoBehaviour
{
    private bool isLoading;

    [Header("Reference Fields")]
    public GameObject FirstChampionDisplayReference;
    public GameObject SecondChampionDisplayReference;
    public GameObject TeamDisplayReference;

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
        if (firstChampion != null)
        {
            var firstChampionWinrate = await ApiClient.GetChampionWinrate(firstChampion);
            var firstChampionAlternativeBuilds = await ApiClient.GetChampionAlternativeBuilds(firstChampion, possibleItemSets);
            var firstChampionDisplay = FirstChampionDisplayReference.GetComponent<ChampionInformationDisplay>();
            firstChampionDisplay.Initialize(firstChampionWinrate, firstChampionAlternativeBuilds);
        }

        if (secondChampion != null)
        {
            var secondChampionWinrate = await ApiClient.GetChampionWinrate(secondChampion);
            var secondChampionAlternativeBuilds = await ApiClient.GetChampionAlternativeBuilds(secondChampion, possibleItemSets);
            var secondChampionDisplay = SecondChampionDisplayReference.GetComponent<ChampionInformationDisplay>();
            secondChampionDisplay.Initialize(secondChampionWinrate, secondChampionAlternativeBuilds);
        }

        var teamWinrate = await ApiClient.GetTeamWinrate(team);
        var alternativeTeamComps = await ApiClient.GetTeamAlternativeComps(team, relaventChampions);

        Debug.Log($"POSSIBLE ITEM SETS: {possibleItemSets.Count}");
        Debug.Log($"RELAVENT CHAMPIONS: {relaventChampions.Count}");
        Debug.Log($"ALTERNATIVE TEAM COMPS LISTING: {alternativeTeamComps.Count}");
        Debug.Log("GOT HERE!");

    }


}