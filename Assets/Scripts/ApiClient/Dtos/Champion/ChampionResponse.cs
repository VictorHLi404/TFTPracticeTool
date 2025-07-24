using System.Collections.Generic;

public class ChampionResponse
{
    public ChampionEnum ChampionName { get; set; }
    public decimal AveragePlacement { get; set; }
    public int Level { get; set; }
    public List<AllItemsEnum> Items { get; set; }
}