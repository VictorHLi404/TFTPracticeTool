using UnityEngine;

public class ChampionIcon : MonoBehaviour
{

    public void updateChampionImage(UnitData champion)
    {
        /*
        Given the unitdata of a champion, extract its name, and replace sprite of image.
        */
        SpriteRenderer spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        string unitName = champion.UnitName.Replace(".", "");
        string file_path = $"ChampionShopIcons/TFT14_{unitName}.TFT_Set14";
        spriteRendererComponent.sprite = Resources.Load<Sprite>(file_path);
    }

}