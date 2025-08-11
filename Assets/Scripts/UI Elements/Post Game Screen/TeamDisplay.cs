using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TeamDisplay : MonoBehaviour
{
    [Header("Reference Fields")]
    public GameObject ChampionHexListingReference;
    public TMP_Text AveragePlacementReference;
    public List<GameObject> AlternativeTeamDisplayReferences;

    public void Initialize(List<Champion> team, TeamResponse teamStatistics, List<TeamResponse> alternativeTeams)
    {
        var initialTeamStatistics = StartingResources.Instance.initialTeamStatistics;
        var championHexListing = ChampionHexListingReference.GetComponent<ChampionHexListing>();
        championHexListing.Initialize(team);
        if (teamStatistics != null)
        {
            Debug.Log(initialTeamStatistics);
            AveragePlacementReference.text = GetWinrateDifferenceString(teamStatistics.AveragePlacement, initialTeamStatistics.AveragePlacement);
            if (teamStatistics.AveragePlacement < initialTeamStatistics.AveragePlacement)
                AveragePlacementReference.color = Color.green;
            else
                AveragePlacementReference.color = Color.red;
        }
        else
        {
            AveragePlacementReference.text = "We could not find statistics for this team.";
            AveragePlacementReference.fontSize = 20;
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
        if (teamAveragePlacement <= expectedAveragePlacement)
            return $"{(Math.Truncate(teamAveragePlacement * 100) / 100).ToString()} (-{(Math.Truncate(Math.Abs(teamAveragePlacement - expectedAveragePlacement) * 100) / 100).ToString()})";
        else
            return $"{(Math.Truncate(teamAveragePlacement * 100) / 100).ToString()} (+{(Math.Truncate(Math.Abs(expectedAveragePlacement - teamAveragePlacement) * 100) / 100).ToString()})";
    }
    public static string GetWinrateDifferenceStringWithNewLine(decimal teamAveragePlacement, decimal expectedAveragePlacement)
    {
        if (teamAveragePlacement <= expectedAveragePlacement)
            return $"{(Math.Truncate(teamAveragePlacement * 100) / 100).ToString()}\n(-{(Math.Truncate(Math.Abs(teamAveragePlacement - expectedAveragePlacement) * 100) / 100).ToString()})";
        else
            return $"{(Math.Truncate(teamAveragePlacement * 100) / 100).ToString()}\n(+{(Math.Truncate(Math.Abs(expectedAveragePlacement - teamAveragePlacement) * 100) / 100).ToString()})";
    }
}