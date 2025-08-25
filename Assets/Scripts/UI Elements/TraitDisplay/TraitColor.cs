using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that handles the trait display backing color. Takes in an enum value to determine what to show.
/// </summary>
public class TraitColor : MonoBehaviour
{
    public void UpdateTraitColor(TraitRarities rarity)
    {
        SpriteRenderer spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        string filePath = GetTraitFilePath(rarity);
        spriteRendererComponent.sprite = Resources.Load<Sprite>(filePath);
    }

    public void UpdateTraitColorCanvas(TraitRarities rarity)
    {
        Image imageComponent = gameObject.GetComponent<Image>();
        string filePath = GetTraitFilePath(rarity);
        imageComponent.sprite = Resources.Load<Sprite>(filePath);
    }

    private string GetTraitFilePath(TraitRarities rarity)
    {
        string filePath = "";
        switch (rarity)
        {
            case TraitRarities.Starting:
                filePath = "CustomArtAssets/StartingLevelTrait";
                break;
            case TraitRarities.Bronze:
                filePath = "CustomArtAssets/BronzeLevelTrait";
                break;
            case TraitRarities.Silver:
                filePath = "CustomArtAssets/SilverLevelTrait";
                break;
            case TraitRarities.Gold:
                filePath = "CustomArtAssets/GoldLevelTrait";
                break;
            case TraitRarities.Prismatic:
                filePath = "CustomArtAssets/PrismaticLevelTrait";
                break;
            case TraitRarities.Unique:
                filePath = "CustomArtAssets/UniqueLevelTrait";
                break;
        }
        return filePath;
    }
}