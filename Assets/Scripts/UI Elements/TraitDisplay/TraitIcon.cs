using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that handles picking the icon to display for traits. Takes in a string argument as the trait name,
/// and handles accordingly.
/// </summary>
public class TraitIcon : MonoBehaviour
{
    public void UpdateTraitIcon(string traitName, bool isActive)
    {
        SpriteRenderer spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        var pathArgument = traitName.Replace(" ", "");
        var filePath = $"TraitIcons/{pathArgument}_Icon";
        spriteRendererComponent.sprite = Resources.Load<Sprite>(filePath);
        if (isActive)
        {
            spriteRendererComponent.color = new Color32(0, 0, 0, 255);
        }
        else
        {
            spriteRendererComponent.color = new Color32(94, 94, 96, 255);
        }
    }

    public void UpdateTraitIconCanvas(string traitName, bool isActive)
    {
        Image imageComponent = gameObject.GetComponent<Image>();
        var pathArgument = traitName.Replace(" ", "");
        var filePath = $"TraitIcons/{pathArgument}_Icon";
        imageComponent.sprite = Resources.Load<Sprite>(filePath);
        if (isActive)
        {
            imageComponent.color = new Color32(0, 0, 0, 255);
        }
        else
        {
            imageComponent.color = new Color32(94, 94, 96, 255);
        }
    }

    public void UpdateShopTraitIcon(string traitName)
    {
        SpriteRenderer spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        string pathArgument = traitName.Replace(" ", "");
        string file_path = $"TraitIcons/{pathArgument}_Icon";
        spriteRendererComponent.sprite = Resources.Load<Sprite>(file_path);
        spriteRendererComponent.color = new Color32(255, 255, 255, 255);
    }
}