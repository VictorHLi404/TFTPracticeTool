using UnityEngine.UI;
using UnityEngine;

public class ChampionIcon : MonoBehaviour
{

    public void updateChampionImage(UnitData champion)
    {
        /*
        Given the unitdata of a champion, extract its name, and replace sprite of image.
        */
        SpriteRenderer spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        string unitName = champion.UnitName.ToString().Replace(".", "");
        string filePath = $"ChampionShopIcons/TFT15_{unitName}.TFT_Set15";
        spriteRendererComponent.sprite = Resources.Load<Sprite>(filePath);
    }

    public void UpdateCanvasChampionImage(UnitData champion)
    {
        Image imageComponent = gameObject.GetComponent<Image>();
        string unitName = champion.UnitName.ToString().Replace(".", "");
        string filePath = $"ChampionShopIcons/TFT15_{unitName}.TFT_Set15";
        imageComponent.sprite = Resources.Load<Sprite>(filePath);
    }

}