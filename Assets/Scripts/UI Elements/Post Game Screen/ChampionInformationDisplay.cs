using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChampionInformationDisplay : MonoBehaviour
{
    [Header("Reference Fields")]
    public TMP_Text ChampionNameReference;
    public GameObject ChampionDisplayHexReference;
    public GameObject ItemDisplayReference;
    public TMP_Text AveragePlacementReference;
    public List<GameObject> ItemSuggestionsReferences;

    public void Initialize(ChampionResponse championStats, List<ChampionResponse> alternativeBuilds)
    {
        var champion = new Champion(championStats.Level, new UnitData
        {
            UnitName = championStats.ChampionName,
        });
        var championDisplayHex = ChampionDisplayHexReference.GetComponent<ChampionDisplayHex>();
        championDisplayHex.UpdateVisuals(champion);
        ChampionNameReference.text = championStats.ChampionName.ToString();
        AveragePlacementReference.text = (Math.Truncate(championStats.AveragePlacement * 100) / 100).ToString();
        var itemDisplay = ItemDisplayReference.GetComponent<ItemDisplay>();
        itemDisplay.Initialize(championStats.Items);

        var itemSuggestionsCount = Math.Min(alternativeBuilds.Count, 3);
        for (int i = 0; i < itemSuggestionsCount; i++)
        {
            var itemSuggestion = ItemSuggestionsReferences[i].GetComponent<ItemSuggestion>();
            itemSuggestion.Initialize(alternativeBuilds[i]);
        }
        if (itemSuggestionsCount < 3)
        {
            for (int i = itemSuggestionsCount; i < 3; i++)
            {
                ItemSuggestionsReferences[i].SetActive(false);
            }
        }
    }
}