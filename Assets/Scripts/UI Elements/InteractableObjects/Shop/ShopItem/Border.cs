using UnityEngine;

public class Border : MonoBehaviour {

    public void updateColor(UnitData champion) {
        /*
        Given a champion, extract its cost and change the sprite renderer component to the appropriate color.
        */
        SpriteRenderer spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        int cost = champion.Cost;
        string file_path = $"CustomArtAssets/{cost}_Cost_Border";
        spriteRendererComponent.sprite = Resources.Load<Sprite>(file_path);
    }

}