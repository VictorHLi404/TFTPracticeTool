using UnityEngine;

/// <summary>
/// A class that handles the trait display backing color. Takes in an enum value to determine what to show.
/// </summary>
public class TraitColor : MonoBehaviour
{
    public void UpdateTraitColor(TFTEnums.TraitRarities rarity)
    {
        SpriteRenderer spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        string filePath = "";
        switch (rarity)
        {
            case TFTEnums.TraitRarities.Starting:
                filePath = "CustomArtAssets/StartingLevelTrait";
                break;
            case TFTEnums.TraitRarities.Bronze:
                filePath = "CustomArtAssets/BronzeLevelTrait";
                break;
            case TFTEnums.TraitRarities.Silver:
                filePath = "CustomArtAssets/SilverLevelTrait";
                break;
            case TFTEnums.TraitRarities.Gold:
                filePath = "CustomArtAssets/GoldLevelTrait";
                break;
            case TFTEnums.TraitRarities.Prismatic:
                filePath = "CustomArtAssets/PrismaticLevelTrait";
                break;
            case TFTEnums.TraitRarities.Unique:
                filePath = "CustomArtAssets/UniqueLevelTrait";
                break;
        }
        spriteRendererComponent.sprite = Resources.Load<Sprite>(filePath);
    }
}