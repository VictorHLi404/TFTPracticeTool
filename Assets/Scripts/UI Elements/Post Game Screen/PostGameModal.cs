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
    public GameObject LoadingScreenReference;
    public GameObject LoadedContentReference;
    private bool requestsHaveBeenMade;

    public async UniTask Initialize(List<ChampionEntity> teamChampions,
                            List<(UnitData unit, int occurences)> shopChampions,
                            List<Component> initialComponents)
    {
        LoadedContentReference.SetActive(false);
        LoadingScreenReference.SetActive(true);
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
            firstChampionDisplay.Initialize(firstChampion, firstChampionWinrate, firstChampionAlternativeBuilds);
        }

        if (secondChampion != null)
        {
            var secondChampionWinrate = await ApiClient.GetChampionWinrate(secondChampion);
            var secondChampionAlternativeBuilds = await ApiClient.GetChampionAlternativeBuilds(secondChampion, possibleItemSets);
            var secondChampionDisplay = SecondChampionDisplayReference.GetComponent<ChampionInformationDisplay>();
            secondChampionDisplay.Initialize(secondChampion, secondChampionWinrate, secondChampionAlternativeBuilds);
        }

        var teamWinrate = await ApiClient.GetTeamWinrate(team);
        var alternativeTeamComps = await ApiClient.GetTeamAlternativeComps(team, relaventChampions);

        var teamDisplay = TeamDisplayReference.GetComponent<TeamDisplay>();
        teamDisplay.Initialize(team, teamWinrate, alternativeTeamComps);

        LoadingScreenReference.SetActive(false);
        LoadedContentReference.SetActive(true);
    }


}