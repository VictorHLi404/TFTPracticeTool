using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {

    

    public int shopSlots = 5; // number of shop slots
    public float slotWidth = 150f;  // Exact width of the hex tile
    public float slotHeight = 100f; // Exact height of the hex tile 
    public float spacing = 25f;      // Spacing between bench slots

    public float buttonXSpacing = 30f;
    public float buttonYSpacing = 35f;
    [Header("Shop Settings")]
    public GameObject shopSlotPrefab; // Prefab for the shop slot tile
    public GameObject rerollButtonPrefab; 
    public GameObject XPButtonPrefab; 
    
    public GameObject textFieldPrefab; // prefab for text fields

    private Shop shop;
    private Player player;

    private GameObject[] shopItemArray = new GameObject[5];


    private void Start()
    {
        if (shopSlotPrefab == null || rerollButtonPrefab == null || XPButtonPrefab == null) {
            Debug.LogError("Tile prefab not assigned. Please assign it in the inspector.");
            return;
        }
        shop = new Shop();
        shop.generateShop();
        player = new Player(7, 50, 40, 4, 2);
        GenerateShopUI();
    }

    private void GenerateShopUI() {

        float slotSpacing = slotWidth + spacing; // Horizontal spacing between slots

        // Calculate total bench width
        float benchWidth = (shopSlots - 1) * slotSpacing + slotWidth;
        float benchOriginX = -benchWidth / 200f + (slotWidth / 200f) - 237f;
        float benchOriginY = transform.position.y - 20f; // Vertical offset (adjusted upwards)

        for (int i = 0; i < shopSlots; i++)
        {
            // Calculate the position of each bench slot
            float xPos = i * slotSpacing + benchOriginX;
            float yPos = benchOriginY;
            Debug.Log($"{xPos}, {yPos}");
            
            // Instantiate the bench slot

            GameObject newShopSlot = Instantiate(shopSlotPrefab, new Vector3(xPos, yPos, this.transform.position.z-10), Quaternion.identity, transform);
            shopItemArray[i] = newShopSlot;
            ShopItem slotData = newShopSlot.GetComponents<ShopItem>()[0];
            slotData.updateChampion(shop.currentShop[i]);
        }
        Instantiate(XPButtonPrefab, new Vector3(benchOriginX - slotWidth - buttonXSpacing, benchOriginY + buttonYSpacing, 0), Quaternion.identity, transform);
        Instantiate(rerollButtonPrefab, new Vector3(benchOriginX - slotWidth - buttonXSpacing, benchOriginY - buttonYSpacing, 0), Quaternion.identity, transform);
        Instantiate(textFieldPrefab, new Vector3(benchOriginX, benchOriginY, -1), Quaternion.identity, transform);
    }
}