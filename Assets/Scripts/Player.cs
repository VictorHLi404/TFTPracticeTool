using System;

public class Player
{
    // Properties
    public int Level { get; set; }
    public int Gold { get; set; }
    public float Time { get; set; }
    public int Stage { get; set; }
    public int Round { get; set; }

    // Constructors
    public Player(int level, int gold, float time, int stage, int round)
    {
        this.Level = level;
        this.Gold = gold;
        this.Time = time;
        this.Stage = stage;
        this.Round = round;
    }

    // Methods
    public void UpdateGold(int amount)
    {
        Gold += amount;
    }

    public void AdvanceStage()
    {
        Stage++;
        Round = 1; // Resest round to 1 everytime stage is increased
    }
}