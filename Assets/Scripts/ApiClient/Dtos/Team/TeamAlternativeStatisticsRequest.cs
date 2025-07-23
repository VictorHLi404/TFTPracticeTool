using System.Collections.Generic;

public class TeamAlternativeStatisticsRequest
{
    public TeamRequest Team { get; set; }
    public List<ChampionRequest> AlternativeChampions { get; set; }
}