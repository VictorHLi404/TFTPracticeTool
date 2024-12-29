using UnityEngine;
using static DatabaseAPI;
public class DatabaseButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // assign to a random button to rebuild the database from given csv files
    void Start()
    {
        
    }

    private void OnMouseDown() {
        /*DatabaseBuilder.generateNewDatabase();
        DatabaseBuilder.buildChampionTable();
        Debug.Log("Champion Table generated!");
        DatabaseBuilder.buildShopOdds();
        Debug.Log("Shop Odds generated!");
        DatabaseBuilder.buildDefaultBagSizes();
        Debug.Log("Default Bag Sizes generated!");
        DatabaseBuilder.buildTraitLevels();
        Debug.Log("Trait Levels generated!");*/
        UnitData testRun = DatabaseAPI.getUnitData("Violet");
        Debug.Log(testRun);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
