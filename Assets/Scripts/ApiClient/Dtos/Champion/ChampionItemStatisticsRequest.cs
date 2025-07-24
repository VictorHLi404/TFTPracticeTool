using System.Collections.Generic;

public class ChampionItemStatisticsRequest
{
    public ChampionRequest MainChampion { get; set; }
    public List<List<AllItemsEnum>> PossibleItemSets { get; set; }
}