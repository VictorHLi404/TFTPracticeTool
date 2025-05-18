using System.Collections.Generic;
using UnityEngine;
using static DatabaseAPI;
public class DatabaseButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // assign to a random button to rebuild the database from given csv files
    void Start()
    {

    }

    private void OnMouseDown()
    {
        Shop newShop = new Shop();
        newShop.generateShop(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
