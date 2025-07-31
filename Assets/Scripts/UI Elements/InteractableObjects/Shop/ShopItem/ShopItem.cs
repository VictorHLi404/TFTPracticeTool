using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{


    public bool isActive = true;
    private UnitData champion;
    private GameObject border;
    private GameObject championIcon;
    private ShopUI parentShop;

    [Header("Object References")]

    public GameObject traitDisplayTemplate;



    public void Start()
    {
        this.border = transform.Find("Border").gameObject;
        this.championIcon = transform.Find("ChampionIcon").gameObject;
    }

    public ShopItem()
    {
        this.champion = null;
    }

    public ShopItem(UnitData _champion)
    {
        this.champion = _champion;
    }

    public void setParentShop(ShopUI _parentShop)
    {
        this.parentShop = _parentShop;
    }

    public void updateChampion(UnitData newChampion)
    {
        enableInteraction(true);
        this.champion = newChampion;
        this.border = transform.Find("Border").gameObject;
        border.GetComponent<Border>().updateColor(newChampion);

        GameObject championNameTextField = transform.Find("ChampionNameField").gameObject;
        championNameTextField.GetComponent<TextMeshPro>().text = newChampion.UnitName.ToString();

        GameObject costTextField = transform.Find("CostTextField").gameObject;
        costTextField.GetComponent<TextMeshPro>().text = $"{newChampion.Cost}";

        this.championIcon = transform.Find("ChampionIcon").gameObject;
        championIcon.GetComponent<ChampionIcon>().updateChampionImage(newChampion);


        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ShopTraitDisplay>())
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        int traitCount = champion.UnitTraits.Count;
        float yPosition = -2 + ((traitCount-1) * 1.5f);

        foreach (string traitName in champion.UnitTraits)
        {
            GameObject traitDisplayField = Instantiate(traitDisplayTemplate, transform);
            traitDisplayField.transform.localPosition = new Vector3(-5.5f, yPosition, -0.5f);
            traitDisplayField.transform.localScale = new Vector3(1.5f, 1.5f, 1f);

            traitDisplayField.GetComponent<ShopTraitDisplay>().Initialize(traitName);
            yPosition -= 1.5f;
        }

    }

    public void enableInteraction(bool state)
    {
        isActive = state;
        gameObject.SetActive(state);
    }

    public void purchaseChampion()
    {
        if (parentShop.BuyChampion(champion, gameObject))
        {
            enableInteraction(false);

        }
    }

    public void OnMouseDown()
    {
        purchaseChampion();
    }

}