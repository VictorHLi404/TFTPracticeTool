using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopUI : MonoBehaviour
{

    [Header("Shop Settings")]
    public int shopSlots = 5; // number of shop slots
    public float slotWidth = 150f;  // Exact width of the hex tile
    public float slotHeight = 100f; // Exact height of the hex tile 
    public float spacing = 25f;      // Spacing between bench slots

    public GameObject shopSlotPrefab; // Prefab for the shop slot tile
    private GameObject XPButton;
    private GameObject rerollButton;
    private GameObject levelTextField;
    private GameObject XPTextField;
    private GameObject XPBar;
    private GameObject GoldTextField;
    private GameObject ShopOddsTextField;
    private GameObject SellChampionInformation;
    private Shop shop;
    private GameObject[] shopItemArray = new GameObject[5];
    private bool[] hasBeenPurchased = new bool[5]; // backing array to keep track of which are actually empty and which aren't

    [Header("UI Reference Objects")]
    public GameObject benchManager;

    private void Start()
    {
        shop = new Shop();
        shop.generateShop(true);
        GenerateButtons();
        GenerateDisplayElements();
        GenerateShopItems();
    }

    /// <summary>
    /// Generate the XP and Reroll Buttons. Attach them to the parent shopUI object.
    /// </summary>
    private void GenerateButtons()
    {
        rerollButton = transform.Find("RerollButton").gameObject;
        XPButton = transform.Find("XPButton").gameObject;
        rerollButton.GetComponent<RerollButton>().setParentShop(this);
        XPButton.GetComponent<XPButton>().setParentShop(this);
    }

    /// <summary>
    /// Attach the display field objects to the parent shopUI fields.
    /// </summary>
    private void GenerateDisplayElements()
    {
        levelTextField = transform.Find("LevelTextField").gameObject;
        XPTextField = transform.Find("XPTextField").gameObject;
        XPBar = transform.Find("XPBarFill").gameObject;
        GoldTextField = transform.Find("GoldTextField").gameObject;
        ShopOddsTextField = transform.Find("ShopOddsTextField").gameObject;
        SellChampionInformation = transform.Find("SellChampionInformation").gameObject;
        SellChampionInformation.SetActive(false);
        UpdateDisplays();
    }

    /// <summary>
    /// Helper method to update player stat displays. call the info gathering method from shop and then update accordingly. 
    /// </summary>
    public void UpdateDisplays()
    {
        (int level, int xp, int xpCap, int gold, List<int> levelOdds) = shop.getDisplayData();
        Debug.Log($"UPDATING GOLD... {gold}");
        levelTextField.GetComponent<TextMeshPro>().text = $"Lvl. {level}";
        XPTextField.GetComponent<TextMeshPro>().text = $"{xp}/{xpCap}";
        GoldTextField.GetComponent<TextMeshPro>().text = $"{gold}";
        UpdateShopOddsDisplay(levelOdds);

        float XPBarMaxLength = transform.Find("XPBarBackground").transform.localScale.x;
        float XPBarDefaultX = transform.Find("XPBarBackground").transform.localPosition.x - (XPBarMaxLength / 2);
        float newXPBarLength = XPBarMaxLength * ((float)xp / (float)xpCap);
        XPBar.transform.localScale = new Vector3(newXPBarLength, XPBar.transform.localScale.y, XPBar.transform.localScale.z);
        XPBar.transform.localPosition = new Vector3(newXPBarLength / 2 + XPBarDefaultX, XPBar.transform.localPosition.y, XPBar.transform.localPosition.z);
    }

    /// <summary>
    /// Helper method to update the shop odds display. Takes in a list of shop odds where the index corresponds
    /// to the appropriate unit cost.
    /// </summary>
    /// <param name="levelOdds">List of odds for a unit cost.</param>
    public void UpdateShopOddsDisplay(List<int> levelOdds)
    {
        for (int i = 1; i <= 5; i++)
        {
            string textFieldName = $"{i}CostOddsTextField";
            ShopOddsTextField.transform.Find(textFieldName).GetComponent<TextMeshPro>().text = $"{levelOdds[i - 1]}%";
        }
    }
    /// <summary>
    /// Instantiate the shop UI elements. 
    /// </summary>
    private void GenerateShopItems()
    {

        float slotSpacing = slotWidth + spacing; // Horizontal spacing between slots

        for (int i = 0; i < shopSlots; i++)
        {
            // Calculate the position of each bench slot
            float xPos = i * slotSpacing - 5.15f; // TODO: MAKE THIS RELATIVE

            // Instantiate the bench slot

            // GameObject newShopSlot = Instantiate(shopSlotPrefab, new Vector3(xPos, yPos, benchOriginZ - 10), Quaternion.identity, transform);
            GameObject newShopSlot = Instantiate(shopSlotPrefab, transform);
            newShopSlot.transform.localPosition = new Vector3(xPos, -0.4f, -20);
            shopItemArray[i] = newShopSlot;
            ShopItem slotData = newShopSlot.GetComponents<ShopItem>()[0];
            slotData.updateChampion(shop.currentShop[i]);
            slotData.setParentShop(this);
            slotData.enableInteraction(true);
        }
    }

    /// <summary>
    /// Reroll the shop by making a call to the internal shop object. Then, update the display fields accordingly.
    /// </summary>
    public void rerollShop()
    {
        if (shop.generateShop(false))
        {
            for (int i = 0; i < shopSlots; i++)
            {
                ShopItem slotData = shopItemArray[i].GetComponents<ShopItem>()[0];
                slotData.updateChampion(shop.currentShop[i]);
            }
            UpdateDisplays();
        }
    }


    /// <summary>
    /// Call the shop to buy XP. if it does so successfully, grab the new updated player data from shop and update relevant text fields.
    /// </summary>
    public void buyXP()
    {
        if (shop.buyXP())
        {
            UpdateDisplays();
        }
    }

    public void sellChampion(ChampionEntity championEntity)
    {
        if (shop.sellChampion(championEntity.champion))
        {
            Debug.Log("SUCESSFULLY SOLD!");
            UpdateDisplays();
            SellChampionInformation.GetComponent<SellChampionInformation>().disableDisplay();
            for (int i = 0; i < shopItemArray.Length; i++)
            {
                if (!hasBeenPurchased[i])
                {
                    shopItemArray[i].GetComponent<ShopItem>().enableInteraction(true);
                }
            }
        }
    }

    /// <summary>
    /// 1. remove that champion from the current shop by making a parent call. 
    /// 2. (TODO) create an instance of the champion on the bench
    /// FLIP THE ORDER: BENCH COMES FIRST, THEN SHOP
    /// </summary>
    /// <param name="champion"></param>
    /// <returns></returns>
    public bool buyChampion(UnitData champion)
    {
        BenchManager bench = benchManager.GetComponent<BenchManager>();
        if (!bench.AddToBench())
        { // no space on bench
            Debug.Log("NO SPACE ON BENCH!");
            return false;
        }
        if (!shop.buyChampion(champion))
        { // no money
            Debug.Log("TOO POOR!");
            return false;
        }
        Champion newChampion = new Champion(1, champion);
        bench.placeInBench(newChampion);
        UpdateDisplays();
        return true;
    }

    public void OnMouseDown()
    {
        Debug.Log("CLICKED ON THE SHOP!");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.GetComponent<ChampionEntity>() != null)
        {
            Debug.Log("CHAMPION ENTERED SHOP FROM POV OF SHOP");
            for (int i = 0; i < shopItemArray.Length; i++)
            {
                ShopItem shopSlot = shopItemArray[i].GetComponent<ShopItem>();
                hasBeenPurchased[i] = !shopSlot.isActive;
            }
            foreach (GameObject shopSlotObject in shopItemArray)
            {
                shopSlotObject.GetComponent<ShopItem>().enableInteraction(false);
            }
            int sellPrice = collisionObject.GetComponent<ChampionEntity>().getSellPrice();
            SellChampionInformation.GetComponent<SellChampionInformation>().enableDisplay(sellPrice);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.GetComponent<ChampionEntity>() != null)
        {
            Debug.Log("CHAMPION LEFT SHOP FROM POV OF SHOP");
            SellChampionInformation.GetComponent<SellChampionInformation>().disableDisplay();
            for (int i = 0; i < shopItemArray.Length; i++)
            {
                if (!hasBeenPurchased[i])
                {
                    shopItemArray[i].GetComponent<ShopItem>().enableInteraction(true);
                }
            }
        }
    }
}