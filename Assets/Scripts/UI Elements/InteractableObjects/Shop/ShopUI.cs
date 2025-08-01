using System;
using System.Collections.Generic;
using TMPro;
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
    public GameObject hexGridManager;
    public GameObject itemBench;

    public void Awake()
    {
        shop = new Shop();
        shop.generateShop(true);
        GenerateButtons();
        GenerateDisplayElements();
        GenerateShopItems();
    }

    public void Start()
    {
        if (StartingResources.Instance.initialChampions != null && StartingResources.Instance.initialChampions.Count > 0)
        {
            GenerateInitialChampions(StartingResources.Instance.initialChampions);
        }
    }

    public void Update()
    {
        hexGridManager.GetComponent<HexGridManager>().updateMaxUnitCount(shop.getPlayerLevel());
        if (Input.anyKeyDown)
        {
            if (Input.GetKey(Settings.Instance.HotkeyBindings[HotkeyEnum.RerollHotkey]))
            {
                RerollShop();
            }
            else if (Input.GetKey(Settings.Instance.HotkeyBindings[HotkeyEnum.BuyXPHotkey]))
            {
                BuyXP();
            }
        }
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
    public void RerollShop()
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
    public void BuyXP()
    {
        if (shop.BuyXP())
        {
            UpdateDisplays();
            hexGridManager.GetComponent<HexGridManager>().updateMaxUnitCount(shop.getPlayerLevel());
        }
    }

    public void SellChampion(ChampionEntity championEntity)
    {
        if (shop.SellChampion(championEntity.champion))
        {
            UpdateDisplays();
            SellChampionInformation.GetComponent<SellChampionInformation>().disableDisplay();
            for (int i = 0; i < shopItemArray.Length; i++)
            {
                if (!hasBeenPurchased[i])
                {
                    shopItemArray[i].GetComponent<ShopItem>().enableInteraction(true);
                }
                else
                {
                    Debug.Log($"SLOT {i} HAS BEEN PURCHASED");
                }
            }
        }
        itemBench.GetComponent<ItemManager>().ReturnItemsToBench(championEntity.champion.GetItems());
    }

    /// <summary>
    /// 1. remove that champion from the current shop by making a parent call. 
    /// 2. (TODO) create an instance of the champion on the bench
    /// FLIP THE ORDER: BENCH COMES FIRST, THEN SHOP
    /// </summary>
    /// <param name="champion"></param>
    /// <returns></returns>
    public bool BuyChampion(UnitData champion, GameObject shopItem)
    {
        BenchManager bench = benchManager.GetComponent<BenchManager>();
        if (!bench.CanUnitBePlaced())
        { // no space on bench
            return false;
        }
        if (!shop.buyChampion(champion))
        { // no money
            return false;
        }
        int shopItemIndex = -1;
        for (int i = 0; i < shopItemArray.Length; i++)
        {
            if (shopItem == shopItemArray[i])
            {
                shopItemIndex = i;
                break;
            }
        }
        if (shopItemIndex == -1)
        {
            Debug.LogError("Could not find matching shopitemIndex");
        }
        ShopItem shopSlot = shopItem.GetComponent<ShopItem>();
        hasBeenPurchased[shopItemIndex] = true;
        Champion newChampion = new Champion(1, champion);
        bench.PlaceInBench(newChampion);
        UpdateDisplays();
        return true;
    }

    private void GenerateInitialChampions(List<Champion> champions)
    {
        BenchManager bench = benchManager.GetComponent<BenchManager>();
        int index = 1;
        foreach (var champion in champions)
        {
            index += 1;
            if (!bench.CanUnitBePlaced())
                break;
            for (int i = 0; i < Math.Pow(3, champion.starLevel - 1); i++)
            {
                shop.RemoveChampionFromPool(champion);
            }
            bench.PlaceInBench(champion);
        }
        if (index < champions.Count)
        {
            HexGridManager manager = hexGridManager.GetComponent<HexGridManager>();
            manager.PlaceInBoard(champions[index]);
        }
    }

    public Player GetPlayer()
    {
        return shop.GetPlayer();
    }

    public List<(UnitData unit, int occurences)> GetChampionOccurrences()
    {
        return shop.GetChampionOccurrences();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.GetComponent<ChampionEntity>() != null)
        {
            foreach (GameObject shopSlotObject in shopItemArray)
            {
                shopSlotObject.GetComponent<ShopItem>().enableInteraction(false);
            }
            int sellPrice = collisionObject.GetComponent<ChampionEntity>().GetSellPrice();
            SellChampionInformation.GetComponent<SellChampionInformation>().enableDisplay(sellPrice);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.GetComponent<ChampionEntity>() != null)
        {
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