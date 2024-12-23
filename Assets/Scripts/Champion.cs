using UnityEngine;

public class Champion
{
    public float x,y;
    public int tileType;
    

    public Champion(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public void SetTileType(int tileType) {
        this.tileType = tileType;

    }
 }
