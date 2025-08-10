using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChampionInformationDisplay : MonoBehaviour
{
    [Header("Reference Fields")]
    public TMP_Text ChampionNameReference;
    public GameObject ChampionDisplayHexReference;
    public GameObject ItemDisplayReference;
    public TMP_Text AveragePlacementReference;
    public List<GameObject> ItemSuggestionsReferences;

    public void Initialize(Champion champion, ChampionResponse championStats, List<ChampionResponse> alternativeBuilds)
    {
        var championDisplayHex = ChampionDisplayHexReference.GetComponent<ChampionDisplayHex>();
        championDisplayHex.UpdateVisuals(champion);
        ChampionNameReference.text = champion.UnitName.ToString().ToUpper();
        var items = champion.GetItems();
        var itemEnums = new List<AllItemsEnum>();
        foreach (var item in items)
        {
            AllItemsEnum itemEnum;
            if (Enum.TryParse(item.ToString(), out itemEnum))
                itemEnums.Add(itemEnum);
        }
        var itemDisplay = ItemDisplayReference.GetComponent<ItemDisplay>();
        itemDisplay.Initialize(itemEnums);

        if (championStats != null)
        {
            AveragePlacementReference.text = GetItemizationDifferenceString(championStats.ItemizationAveragePlacement, championStats.ChampionAveragePlacement);

            if (championStats.ItemizationAveragePlacement < championStats.ChampionAveragePlacement)
                AveragePlacementReference.color = Color.green;
            else
                AveragePlacementReference.color = Color.red;
        }
        else
        {
            AveragePlacementReference.text = "We couldn't find statistics for this build.";
            AveragePlacementReference.fontSize = 15;
        }
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

    public static string GetItemizationDifferenceString(decimal itemizationAveragePlacement, decimal championAveragePlacement)
    {
        if (itemizationAveragePlacement <= championAveragePlacement)
            return $"{(Math.Truncate(itemizationAveragePlacement * 100) / 100).ToString()} (-{(Math.Truncate(Math.Abs(itemizationAveragePlacement - championAveragePlacement) * 100) / 100).ToString()})";
        else
            return $"{(Math.Truncate(itemizationAveragePlacement * 100) / 100).ToString()} (+{(Math.Truncate(Math.Abs(championAveragePlacement - itemizationAveragePlacement) * 100) / 100).ToString()})";
    }
}