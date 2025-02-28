using UnityEngine;

public class ChampionShopIcon : MonoBehaviour {

    public void updateChampionImage(UnitData champion) {
        /*
        Given the unitdata of a champion, extract its name, and replace sprite of image.
        */
        SpriteRenderer spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        string unitName = champion.UnitName;
        string file_path = $"ChampionShopIcons/TFT13_{unitName}.TFT_Set13";
        Debug.Log(file_path);
        spriteRendererComponent.sprite = Resources.Load<Sprite>(file_path);
    }

}