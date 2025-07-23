using System.Collections.Generic;

public class ChampionItemStatisticsRequest
{
    public ChampionRequest MainChampion { get; set; }
    public List<AllItemsEnum> items { get; set; }
}