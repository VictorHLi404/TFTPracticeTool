using UnityEngine;

public class Hex
{
    public float x,y;
    public int tileType;

    public Hex(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public void SetTileType(int tileType) {
        this.tileType = tileType;

    }
 }
