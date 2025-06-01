using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;


/// <summary>
/// A class that represents the trait display UI icon. ingests trait information (name, level),
/// and assigns new values to respective and appropriate subfields (icon, backing, level, etc)
/// </summary>
public class TraitDisplay : MonoBehaviour
{

    private int unitCount;
    private string traitName;
    private List<int> traitBreakpoints;
    private List<TFTEnums.TraitRarities> traitRarities;
    private GameObject traitIcon;
    private GameObject traitColor;
    private GameObject traitNameField;
    private GameObject traitCountField;
    private GameObject traitCountBackground;
    private GameObject traitLevelsField;

    public void Initialize(string traitName, int unitCount, List<int> traitBreakpoints, List<TFTEnums.TraitRarities> traitRarities)
    {
        traitIcon = transform.Find("TraitIcon").gameObject;
        traitColor = transform.Find("TraitColor").gameObject;
        traitNameField = transform.Find("TraitNameField").gameObject;
        traitCountField = transform.Find("TraitCountField").gameObject;
        traitCountBackground = transform.Find("TraitCountBackground").gameObject;
        traitLevelsField = transform.Find("TraitLevelsField").gameObject;
        this.traitName = traitName;
        this.unitCount = unitCount;
        this.traitBreakpoints = traitBreakpoints;
        this.traitRarities = traitRarities;
        UpdateDisplays();

    }

    private void UpdateDisplays()
    {
        if (traitBreakpoints.Count < 1 || traitRarities.Count < 1)
        {
            Debug.LogError("Trait breakpoints or trait rarities list formatted incorrectly.");
        }
        if (unitCount >= traitBreakpoints[0]) // trait is active
        {
            traitIcon.GetComponent<TraitIcon>().UpdateTraitIcon(traitName, true);
            int index = 0;
            while (unitCount >= traitBreakpoints[index + 1])
            {
                index += 1;
            }
            TFTEnums.TraitRarities rarity = traitRarities[index];
            traitColor.GetComponent<TraitColor>().UpdateTraitColor(rarity);
            traitCountField.GetComponent<TextMeshPro>().text = $"{unitCount}";
            traitNameField.GetComponent<TextMeshPro>().text = $"<color=\"white\">{traitName}</color>";
            if (traitBreakpoints.Count <= 5)
            {
                traitLevelsField.GetComponent<TextMeshPro>().text = buildTraitLevelString(index);
            }
            else
            {
                traitLevelsField.GetComponent<TextMeshPro>().text = $"<color=\"white\">{unitCount} -  {traitBreakpoints[traitBreakpoints.Count - 1]}</color>";
            }
        }
        else
        {
            traitIcon.GetComponent<TraitIcon>().UpdateTraitIcon(traitName, false);
            traitColor.GetComponent<TraitColor>().UpdateTraitColor(TFTEnums.TraitRarities.Starting);
            Destroy(traitCountBackground);
            Destroy(traitCountField);
            traitNameField.GetComponent<TextMeshPro>().text = traitName;
            traitLevelsField.GetComponent<TextMeshPro>().text = $"{unitCount} / {traitBreakpoints[0]}";
            traitNameField.transform.localPosition = new Vector3(17.5f, 5f, 0f);
            traitLevelsField.transform.localPosition = new Vector3(17.5f, -3f, 0f);

        }

    }

    private string buildTraitLevelString(int index)
    {
        string traitLevelString = "";
        for (int i = 0; i < traitBreakpoints.Count; i++)
        {
            if (i != 0)
            {
                traitLevelString += " > ";
            }
            if (i == index)
            {
                traitLevelString += $"<color=\"white\">{traitBreakpoints[i]}</color> ";
            }
            else
            {
                traitLevelString += $"{traitBreakpoints[i]} ";
            }
        }
        return traitLevelString;
    }

}