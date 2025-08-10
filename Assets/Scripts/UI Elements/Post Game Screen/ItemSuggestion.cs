using System;
using TMPro;
using UnityEngine;

public class ItemSuggestion : MonoBehaviour
{
    [Header("Reference Fields")]
    public GameObject ItemDisplayReference;
    public TMP_Text AveragePlacementReference;

    public void Initialize(ChampionResponse championStats)
    {
        AveragePlacementReference.text = ChampionInformationDisplay.GetItemizationDifferenceString(championStats.ItemizationAveragePlacement, championStats.ChampionAveragePlacement);
        if (championStats.ItemizationAveragePlacement < championStats.ChampionAveragePlacement)
            AveragePlacementReference.color = Color.green;
        else
            AveragePlacementReference.color = Color.red;

        var itemDisplay = ItemDisplayReference.GetComponent<ItemDisplay>();
        itemDisplay.Initialize(championStats.Items);
    }
}