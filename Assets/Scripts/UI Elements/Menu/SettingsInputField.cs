using TMPro;
using UnityEngine;

public class SettingsInputField : MonoBehaviour
{

    public TMP_InputField textObject;
    public HotkeyEnum hotkey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Settings.Instance == null)
            Debug.LogError("Settings input field attempted to access settings before initialization, likely race condition");
        textObject.text = Settings.Instance.HotkeyBindings[hotkey].ToString();
    }

    public void SetValue(string value)
    {
        textObject.text = Settings.ValidateHotkey(hotkey, value);
    } 

}
