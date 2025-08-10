using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChampionDisplayHex : MonoBehaviour
{
    [Header("Reference Fields")]
    public GameObject championIcon;
    public GameObject border;

    public void UpdateVisuals(Champion champion, bool isSmall = false)
    {
        Image championImage = championIcon.GetComponent<Image>();
        RectTransform championIconRect = championIcon.GetComponent<RectTransform>();
        championIcon.GetComponent<ChampionIcon>().UpdateCanvasChampionImage(champion);

        // championImage.sprite = CropSprite(championImage.sprite);
        if (isSmall)
            championIconRect.anchoredPosition = new Vector3(championIconRect.anchoredPosition.x + 5f, championIconRect.anchoredPosition.y - 20f);

        int starLevel = champion.starLevel;
        Image borderImage = border.GetComponent<Image>();
        if (starLevel == 1) // bronze
        {
            borderImage.color = new Color(0.8f, 0.5f, 0.2f, 1f);
        }
        else if (starLevel == 2) // silver
        {
            borderImage.color = new Color(0.75f, 0.75f, 0.75f, 1f);
        }
        else // gold
        {
            borderImage.color = new Color(1f, 0.84f, 0f, 1f);
        }
    }
    public Sprite CropSprite(Sprite originalSprite)
    {
        Texture2D originalTexture = originalSprite.texture;

        // Create a new sprite from part of the texture
        Sprite croppedSprite = Sprite.Create(
            originalTexture,
        new Rect(
            x: originalTexture.width * 3 / 10, // Start at middle
            y: 0,                 // From bottom
            width: originalTexture.width * 5 / 10,
            height: originalTexture.height
        ),
            new Vector2(0.2f, 0.5f), // Pivot
            originalSprite.pixelsPerUnit
        );

        return croppedSprite;
    }
}