using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {
    /// generate layer dep
    /// 
    
    private UnitData champion;
    private GameObject border;
    private GameObject championIcon;

    private ShopUI parentShop;

    public void Start() {
        this.border = transform.GetChild(0).gameObject;
        this.championIcon = transform.GetChild(2).gameObject;
    }

    public ShopItem() {
        this.champion = null;
    }

    public ShopItem(UnitData _champion) {
        this.champion = _champion;
    }

    public void setParentShop(ShopUI _parentShop) {
        this.parentShop = _parentShop;
    }

    public void updateChampion(UnitData newChampion) {
        enableInteraction(true);
        this.champion = newChampion;
        this.border = transform.GetChild(0).gameObject;
        this.border.GetComponent<Border>().updateColor(newChampion);

        GameObject textField = transform.GetChild(1).gameObject;
        textField.GetComponent<TextMeshPro>().text =  newChampion.UnitName;

        this.championIcon = transform.GetChild(2).gameObject;
        this.championIcon.GetComponent<ChampionShopIcon>().updateChampionImage(newChampion);

    }

    private void enableInteraction(bool state) {
        gameObject.SetActive(state);
    }

    public void purchaseChampion() {
        if (parentShop.buyChampion(champion)) {
            enableInteraction(false);
        }
    }

    public void OnMouseDown() {
        purchaseChampion();
    }

}