using UnityEngine;

/// <summary>
/// A class that handles picking the icon to display for traits. Takes in a string argument as the trait name,
/// and handles accordingly.
/// </summary>
public class TraitIcon : MonoBehaviour
{
    public void UpdateTraitIcon(string traitName, bool isActive)
    {
        SpriteRenderer spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
        string pathArgument = traitName.Replace(" ", "");
        string file_path = $"TraitIcons/{pathArgument}_Icon";
        spriteRendererComponent.sprite = Resources.Load<Sprite>(file_path);
        if (isActive)
        {
            spriteRendererComponent.color = new Color32(0, 0, 0, 255);
        }
        else {
            spriteRendererComponent.color = new Color32(94, 94, 96, 255);
        }
    }
}