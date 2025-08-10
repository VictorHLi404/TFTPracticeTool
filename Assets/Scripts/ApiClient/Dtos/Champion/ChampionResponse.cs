using System.Collections.Generic;

public class ChampionResponse
{
    public ChampionEnum ChampionName { get; set; }
    public decimal ItemizationAveragePlacement { get; set; }
    public decimal ChampionAveragePlacement { get; set; }
    public int Level { get; set; }
    public List<AllItemsEnum> Items { get; set; }
}