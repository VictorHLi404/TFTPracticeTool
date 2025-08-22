using UnityEngine;

public class Modal : MonoBehaviour
{

    public GameObject UIBlocker;

    public void CloseModal()
    {
        UIBlocker.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
