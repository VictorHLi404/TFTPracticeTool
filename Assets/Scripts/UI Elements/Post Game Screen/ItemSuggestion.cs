using System;
using TMPro;
using UnityEngine;

public class ItemSuggestion : MonoBehaviour
{
    [Header("Reference Fields")]
    public GameObject ItemDisplayReference;
    public TMP_Text AveragePlacement;

    public void Initialize(ChampionResponse championStats)
    {
        AveragePlacement.text = (Math.Truncate(championStats.AveragePlacement * 100) / 100).ToString();
        var itemDisplay = ItemDisplayReference.GetComponent<ItemDisplay>();
        itemDisplay.Initialize(championStats.Items);
    }
}