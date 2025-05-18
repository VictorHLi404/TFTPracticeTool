using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{

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
    private GameObject levelTextField;
    private GameObject XPTextField;
    private GameObject GoldTextField;
    private GameObject XPBar;
    private Shop shop;
    private GameObject[] shopItemArray = new GameObject[5];

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
        updateDisplays();
    }
    /// <summary>
    /// Helper method to update XP displays. call the info gathering method from shop and then update accordingly. 
    /// </summary>
    public void updateDisplays()
    {
        (int level, int xp, int xpCap, int gold) = shop.getDisplayData();
        levelTextField.GetComponent<TextMeshPro>().text = $"Lvl. {level}";
        XPTextField.GetComponent<TextMeshPro>().text = $"{xp}/{xpCap}";
        GoldTextField.GetComponent<TextMeshPro>().text = $"{gold}";

        float XPBarMaxLength = transform.Find("XPBarBackground").transform.localScale.x;
        float XPBarDefaultX = transform.Find("XPBarBackground").transform.localPosition.x - (XPBarMaxLength / 2);
        float newXPBarLength = XPBarMaxLength * ((float)xp / (float)xpCap);
        XPBar.transform.localScale = new Vector3(newXPBarLength, XPBar.transform.localScale.y, XPBar.transform.localScale.z);
        XPBar.transform.localPosition = new Vector3(newXPBarLength / 2 + XPBarDefaultX, XPBar.transform.localPosition.y, XPBar.transform.localPosition.z);
    }
    /// <summary>
    /// Instantiate the shop UI elements. 
    /// </summary>
    private void GenerateShopItems()
    {

        float slotSpacing = slotWidth + spacing; // Horizontal spacing between slots

        // Calculate total bench width
        float benchWidth = (shopSlots - 1) * slotSpacing + slotWidth;
        float benchOriginX = -benchWidth / 200f + (slotWidth / 200f) - 250f;
        float benchOriginY = transform.position.y - 20f; // Vertical offset (adjusted upwards)
        float benchOriginZ = transform.position.z;

        for (int i = 0; i < shopSlots; i++)
        {
            // Calculate the position of each bench slot
            float xPos = i * slotSpacing + benchOriginX;
            float yPos = benchOriginY;

            // Instantiate the bench slot

            GameObject newShopSlot = Instantiate(shopSlotPrefab, new Vector3(xPos, yPos, benchOriginZ - 10), Quaternion.identity, transform);
            shopItemArray[i] = newShopSlot;
            ShopItem slotData = newShopSlot.GetComponents<ShopItem>()[0];
            slotData.updateChampion(shop.currentShop[i]);
            slotData.setParentShop(this);
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
            updateDisplays();
        }
    }


    /// <summary>
    /// Call the shop to buy XP. if it does so successfully, grab the new updated player data from shop and update relevant text fields.
    /// </summary>
    public void buyXP()
    {
        if (shop.buyXP())
        {
            updateDisplays();
        }
    }

    /// <summary>
    /// 1. remove that champion from the current shop by making a parent call. 
    /// 2. (TODO) create an instance of the champion on the bench
    /// </summary>
    /// <param name="champion"></param>
    /// <returns></returns>
    public bool buyChampion(UnitData champion)
    {
        if (shop.buyChampion(champion))
        {
            updateDisplays();
            return true;
        }
        else
        {
            return false;
        }
    }
}