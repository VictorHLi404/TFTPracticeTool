using TMPro;
using UnityEngine;

public class SellChampionInformation : MonoBehaviour
{
    public TMP_Text priceTextField;
    
    public void enableDisplay(int sellPrice)
    {
        transform.gameObject.SetActive(true);
        priceTextField.text = $"{sellPrice}";
    }

    public void disableDisplay()
    {
        transform.gameObject.SetActive(false);
        priceTextField.text = "_";
    }

}