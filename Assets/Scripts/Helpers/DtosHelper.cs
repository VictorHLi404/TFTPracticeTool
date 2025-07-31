using System.Collections.Generic;

public class DtosHelper
{
    public static (decimal expectedPlacement, List<Champion> champions) DeserializeTeamResponse(TeamResponse response)
    {
        var champions = new List<Champion>();
        foreach (var champion in response.Champions)
        {
            var championName = champion.ChampionName.ToString();
            var unitData = DatabaseAPI.getUnitData(championName);
            champions.Add(new Champion(champion.Level, unitData));
        }
        return (response.AveragePlacement, champions);
    }
}