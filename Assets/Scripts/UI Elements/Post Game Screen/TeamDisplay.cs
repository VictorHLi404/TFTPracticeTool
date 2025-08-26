using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamDisplay : MonoBehaviour
{
    [Header("Reference Fields")]
    public GameObject ChampionHexListingReference;
    public GameObject TraitListingReference;
    public TMP_Text AveragePlacementReference;
    public List<GameObject> AlternativeTeamDisplayReferences;

    public void Initialize(List<Champion> team, TeamResponse teamStatistics, List<TeamResponse> alternativeTeams)
    {
        var initialTeamStatistics = StartingResources.Instance.initialTeamStatistics;
        var championHexListing = ChampionHexListingReference.GetComponent<ChampionHexListing>();
        championHexListing.Initialize(team);
        var traitListing = TraitListingReference.GetComponent<TraitListing>();
        traitListing.Initialize(team);
        if (teamStatistics != null && teamStatistics.AveragePlacement != 0 && teamStatistics.Champions.Count != 0)
        {
            Debug.Log(initialTeamStatistics);
            AveragePlacementReference.text = GetWinrateDifferenceString(teamStatistics.AveragePlacement, initialTeamStatistics.AveragePlacement);
            if (teamStatistics.AveragePlacement < initialTeamStatistics.AveragePlacement)
                AveragePlacementReference.color = Color.green;
            else if (teamStatistics.AveragePlacement > initialTeamStatistics.AveragePlacement)
                AveragePlacementReference.color = Color.red;
        }
        else
        {
            AveragePlacementReference.text = "Could not find statistics.";
            AveragePlacementReference.fontSize = 24;
        }
        var teamSuggestionsCount = Math.Min(alternativeTeams.Count, 3);
        for (int i = 0; i < teamSuggestionsCount; i++)
        {
            var teamSuggestion = AlternativeTeamDisplayReferences[i].GetComponent<TeamSuggestion>();
            teamSuggestion.Initialize(alternativeTeams[i]);
        }
        if (teamSuggestionsCount < 3)
        {
            for (int i = teamSuggestionsCount; i < 3; i++)
            {
                AlternativeTeamDisplayReferences[i].SetActive(false);
            }
        }
    }
    public static string GetWinrateDifferenceString(decimal teamAveragePlacement, decimal expectedAveragePlacement)
    {
        if (teamAveragePlacement < expectedAveragePlacement)
            return $"{(Math.Truncate(teamAveragePlacement * 100) / 100):0.00} (-{(Math.Truncate(Math.Abs(teamAveragePlacement - expectedAveragePlacement) * 100) / 100):0.00})";
        else if (teamAveragePlacement > expectedAveragePlacement)
            return $"{(Math.Truncate(teamAveragePlacement * 100) / 100):0.00} (+{(Math.Truncate(Math.Abs(expectedAveragePlacement - teamAveragePlacement) * 100) / 100):0.00})";
        else
            return $"{(Math.Truncate(teamAveragePlacement * 100) / 100):0.00}";
    }
}