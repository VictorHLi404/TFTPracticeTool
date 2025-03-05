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

    private GameObject XPButton;
    private GameObject rerollButton;
    private Shop shop;
    private GameObject[] shopItemArray = new GameObject[5];


    private void Start()
    {
        shop = new Shop();
        shop.generateShop();
        GenerateButtons();
        GenerateShopItems();
    }

    private void GenerateButtons() {
        rerollButton = transform.GetChild(0).gameObject;
        XPButton = transform.GetChild(1).gameObject;
        rerollButton.GetComponent<RerollButton>().setParentShop(this);
        XPButton.GetComponent<XPButton>().setParentShop(this);
    }

    private void GenerateShopItems() {

        float slotSpacing = slotWidth + spacing; // Horizontal spacing between slots

        // Calculate total bench width
        float benchWidth = (shopSlots - 1) * slotSpacing + slotWidth;
        float benchOriginX = -benchWidth / 200f + (slotWidth / 200f) - 237f;
        float benchOriginY = transform.position.y - 20f; // Vertical offset (adjusted upwards)
        float benchOriginZ = transform.position.z;

        for (int i = 0; i < shopSlots; i++) {
            // Calculate the position of each bench slot
            float xPos = i * slotSpacing + benchOriginX;
            float yPos = benchOriginY;
            
            // Instantiate the bench slot

            GameObject newShopSlot = Instantiate(shopSlotPrefab, new Vector3(xPos, yPos, benchOriginZ-10), Quaternion.identity, transform);
            shopItemArray[i] = newShopSlot;
            ShopItem slotData = newShopSlot.GetComponents<ShopItem>()[0];
            slotData.updateChampion(shop.currentShop[i]);
            slotData.setParentShop(this);
        }
    }

    public void rerollShop() {
        if (shop.generateShop()) {
            for (int i = 0; i < shopSlots; i++) {
                ShopItem slotData = shopItemArray[i].GetComponents<ShopItem>()[0];
                slotData.updateChampion(shop.currentShop[i]);
            }
        }
    }

    public void buyXP() {
        shop.buyXP();
    }

    public bool buyChampion(UnitData champion) {
        return shop.buyChampion(champion);
    }
}