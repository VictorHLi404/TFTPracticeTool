using System;

public class Player
{

    public Player(int level, int gold, float time, int stage, int round) {
        this.level = level;
        this.gold = gold;
        this.time = time;
        this.stage = stage;
        this.round = round;
    }


    public int level { get; set; }
    public int gold { get; set; }
    public float time { get; set; }
    public int stage { get; set; }
    public int round { get; set; }
    
    // Constructors

    // Methods
    public void UpdateGold(int amount)
    {
        this.gold += amount;
    }

    public void AdvanceStage()
    {
        this.stage++;
        this.round = 1; // Reset round to 1 every time stage is increased
    }
}