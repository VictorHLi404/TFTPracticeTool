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
        championNameTextField.GetComponent<TextMeshPro>().text = newChampion.UnitName;

        GameObject costTextField = transform.Find("CostTextField").gameObject;
        costTextField.GetComponent<TextMeshPro>().text = $"{newChampion.Cost}";

        this.championIcon = transform.Find("ChampionIcon").gameObject;
        championIcon.GetComponent<ChampionIcon>().updateChampionImage(newChampion);

    }

    public void enableInteraction(bool state)
    {
        isActive = state;
        gameObject.SetActive(state);
    }

    public void purchaseChampion()
    {
        if (parentShop.buyChampion(champion))
        {
            enableInteraction(false);

        }
    }

    public void OnMouseDown()
    {
        Debug.Log("CLICKED TO BUY A CHAMPION");
        purchaseChampion();
    }

}