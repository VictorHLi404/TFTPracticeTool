using UnityEngine;

public class ModalButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject UIBlocker;
    public GameObject Modal;

    void OnMouseDown()
    {
        if (UIBlocker.activeInHierarchy)
            return;

        Debug.Log("CLICKED A MODAL BUTTON!");
        UIBlocker.SetActive(true);
        Modal.SetActive(true);
    }
}
