using UnityEngine;

public class HelpButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject UIBlocker;
    public GameObject HelpModal;

    void OnMouseDown()
    {
        Debug.Log("CLICKED THE HELP BUTTON!");
        UIBlocker.SetActive(true);
        HelpModal.SetActive(true);
    }
}
