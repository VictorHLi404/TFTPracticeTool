using System.Collections.Generic;

public class HexGrid
{
    public float maxSizeTotal;
    public List<Hex> allTiles;
    public Dictionary<string, Hex> map;
    
    public HexGrid(float maxSizeTotal) {
        this.maxSizeTotal = maxSizeTotal;
        allTiles = new List<Hex>();
        map = new Dictionary<string, Hex>();
        // Creating middle tile for starting point
        AddTile(0,0,0);
    }

    public void AddTile(float x, float y, int tileType) {
        Hex hex = new Hex(x,y);
        hex.tileType = tileType;
        allTiles.Add(hex);
        map.Add($"{x},{y}", hex);
    }

    public void AutoGenerateMap() {
        float totalSize = this.maxSizeTotal;
        float currentTotalSize = totalSize; // Counting down to 0
        float currentLayer = 1; // Layer of tiles from middle of map
        int currentTileTypeForLayer = 1; // Used to determine tile type.
        float currentExtraTileForLayer = currentLayer;
        while(currentTotalSize > 0) {
            if (currentTileTypeForLayer < 5) {
                switch (currentTileTypeForLayer) {
                    case 1:
                        AddTile(currentLayer, currentLayer, currentTileTypeForLayer);
                        break;
                    case 2:
                        AddTile(-currentLayer, currentLayer, currentTileTypeForLayer);
                        break;
                    case 3:
                        AddTile(currentLayer, -currentLayer, currentTileTypeForLayer);
                        break;
                    case 4:
                        AddTile(-currentLayer, -currentLayer, currentTileTypeForLayer);
                        break;
                }   
                currentTileTypeForLayer++;
                currentTotalSize--;
            } else {
                switch(currentTileTypeForLayer) {
                    case 5:
                        AddTile(currentLayer + 1 - (2f + currentExtraTileForLayer), 1 + currentLayer, currentTileTypeForLayer);
                        break;
                    case 6:
                        AddTile(1 + currentLayer, currentLayer + 1 - (2f + currentExtraTileForLayer), currentTileTypeForLayer);
                        break;
                    case 7:
                        AddTile(currentLayer + 1 - (2f + currentExtraTileForLayer), -1 - currentLayer, currentTileTypeForLayer);
                        break;
                    case 8:
                        AddTile(-1 - currentLayer, currentLayer + 1 - (2f + currentExtraTileForLayer), currentTileTypeForLayer);
                        break;
                }
                currentExtraTileForLayer--;
                currentTotalSize--;
                if (currentExtraTileForLayer == 0) {
                    currentExtraTileForLayer = currentLayer;
                    currentTileTypeForLayer++;
                }  
            }
            if (currentTileTypeForLayer > 8) {
                currentTileTypeForLayer = 1;
                currentLayer++;
                currentExtraTileForLayer = currentLayer;
            }
        }
    }
}
