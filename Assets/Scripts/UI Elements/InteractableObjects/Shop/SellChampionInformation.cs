using TMPro;
using UnityEngine;

public class SellChampionInformation : MonoBehaviour
{


    private GameObject PriceTextField;
    private void Start()
    {
        this.PriceTextField = transform.Find("PriceTextField").gameObject;
    }

    public void enableDisplay(int sellPrice)
    {
        transform.gameObject.SetActive(true);
        PriceTextField.GetComponent<TextMeshPro>().text = $"{sellPrice}";
    }

    public void disableDisplay()
    {
        transform.gameObject.SetActive(false);
        PriceTextField.GetComponent<TextMeshPro>().text = "_";
    }

}