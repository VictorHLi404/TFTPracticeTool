using System.Collections.Generic;

public class ChampionRequest
{
    public ChampionEnum ChampionName { get; set; }
    public int Level { get; set; }
    public List<AllItemsEnum> Items { get; set; } 
}