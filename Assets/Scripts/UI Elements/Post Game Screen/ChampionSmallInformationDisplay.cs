using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionSmallInformationDisplay : MonoBehaviour
{
    public GameObject ChampionHexReference;
    public TMP_Text ChampionNameReference;

    public void UpdateVisuals(Champion champion, bool isSmall = false)
    {
        ChampionNameReference.text = champion.DisplayName;
        var championHex = ChampionHexReference.GetComponent<ChampionDisplayHex>();
        championHex.UpdateVisuals(champion, isSmall);
    }
}