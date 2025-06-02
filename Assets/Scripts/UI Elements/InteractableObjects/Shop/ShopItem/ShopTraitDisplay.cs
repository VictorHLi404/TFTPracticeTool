using TMPro;
using UnityEngine;

public class ShopTraitDisplay : MonoBehaviour
{
    private GameObject traitIcon;
    private GameObject traitNameField;

    public void Initialize(string traitName)
    {
        traitIcon = transform.Find("ShopTraitIcon").gameObject;
        traitNameField = transform.Find("ShopTraitNameField").gameObject;
        traitIcon.GetComponent<TraitIcon>().UpdateShopTraitIcon(traitName);
        traitNameField.GetComponent<TextMeshPro>().text = traitName;
    }
}