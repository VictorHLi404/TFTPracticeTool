using Cysharp.Threading.Tasks;
using UnityEngine;
public class DatabaseInitializer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // assign to a random button to rebuild the database from given csv files
    public void Awake()
    {
        DatabaseBuilder.initializeDatabase().Forget();
    }
}
