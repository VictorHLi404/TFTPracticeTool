using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamSuggestion : MonoBehaviour
{
    [Header("Reference Fields")]
    public GameObject ChampionHexListingReference;
    public GameObject TraitListingReference;
    public TMP_Text AveragePlacementReference;

    public void Initialize(TeamResponse teamStatistics)
    {
        var initialTeamStatistics = StartingResources.Instance.initialTeamStatistics;
        AveragePlacementReference.text = TeamDisplay.GetWinrateDifferenceString(teamStatistics.AveragePlacement, initialTeamStatistics.AveragePlacement);
        if (teamStatistics.AveragePlacement < initialTeamStatistics.AveragePlacement)
            AveragePlacementReference.color = Color.green;
        else
            AveragePlacementReference.color = Color.red;
        var champions = GenerateChampionsFromResponse(teamStatistics.Champions);
        var championHexListing = ChampionHexListingReference.GetComponent<ChampionHexListing>();
        championHexListing.Initialize(champions);
        var traitListing = TraitListingReference.GetComponent<TraitListing>();
        traitListing.Initialize(champions);
    }

    private List<Champion> GenerateChampionsFromResponse(List<ChampionResponse> championResponses)
    {
        var champions = new List<Champion>();
        foreach (var response in championResponses)
        {
            champions.Add(new Champion(response.Level, DatabaseAPI.GetUnitData(response.ChampionName)));
        }
        return champions;
    }
}