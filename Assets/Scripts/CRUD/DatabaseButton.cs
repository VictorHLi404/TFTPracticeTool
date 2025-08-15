using Cysharp.Threading.Tasks;
using UnityEngine;
public class DatabaseInitializer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // assign to a random button to rebuild the database from given csv files
    public void Awake()
    {
        Debug.Log("WAKE UP THE DATABASE");
        DatabaseBuilder.initializeDatabase().Forget();
    }

    // public void OnMouseDown()
    // {
    //     DatabaseBuilder.initializeDatabase();
    // }

    // // Update is called once per frame
    // public void Update()
    // {

    // }

    // public void InitializeDatabase()
    // {
    //     DatabaseBuilder.initializeDatabase().Forget();
    // }

    // public void OnMouseDown()
    // {
    //     InitializeDatabase();
    // }
}
