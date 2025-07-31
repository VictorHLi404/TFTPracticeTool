using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsInputField : MonoBehaviour
{

    public TMP_InputField textObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (StartingResources.Instance == null)
            Debug.LogError("Settings input field attempted to access settings before initialization, likely race condition");
        textObject.text = "50";
    }

    public void SetValue(string value)
    {
        textObject.text = StartingResources.ValidateGold(value);
    } 

}
