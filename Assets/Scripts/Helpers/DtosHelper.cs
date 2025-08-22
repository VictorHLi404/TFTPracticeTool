using System.Collections.Generic;
using UnityEngine;

public class DtosHelper
{
    public static (decimal expectedPlacement, List<Champion> champions) DeserializeTeamResponse(TeamResponse response)
    {
        var champions = new List<Champion>();
        foreach (var champion in response.Champions)
        {
            var unitData = DatabaseAPI.GetUnitData(champion.ChampionName);
            champions.Add(new Champion(champion.Level, unitData));
        }
        return (response.AveragePlacement, champions);
    }
}