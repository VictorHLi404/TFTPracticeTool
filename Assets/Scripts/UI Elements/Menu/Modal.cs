using UnityEngine;

public class Modal : MonoBehaviour
{

    public GameObject UIBlocker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CloseModal()
    {
        Debug.Log("CLOSE MODAL TRIGGERED");
        UIBlocker.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
