using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; set; }

    public Dictionary<HotkeyEnum, KeyCode> HotkeyBindings = new Dictionary<HotkeyEnum, KeyCode>
    {
        {HotkeyEnum.BuyXPHotkey, KeyCode.B},
        {HotkeyEnum.RerollHotkey, KeyCode.R},
        {HotkeyEnum.SellChampionHotkey, KeyCode.E}
    };
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Instance accessing settings.");
        }
    }

    private static bool CheckDuplicateHotkeys(string newValue)
    {
        KeyCode newKeyCode;
        Enum.TryParse(newValue, true, out newKeyCode);
        foreach (var existingHotkey in Instance.HotkeyBindings.Values)
        {
            if (newKeyCode.Equals(existingHotkey))
            {
                return true;
            }
        }
        return false;
    }

    private static void UpdateHotkey(HotkeyEnum hotkey, string newValue)
    {
        if (string.IsNullOrEmpty(newValue))
        {
            Debug.LogWarning("No value was passed into the hotkey update method. check carefully.");
            return;
        }
        if (CheckDuplicateHotkeys(newValue))
        {
            Debug.LogWarning("A matching value already exists. Ensure to deny this request.");
            return;
        }
        Debug.Log($"UPDATING {hotkey} WITH NEW VALUE {newValue}");
        KeyCode newKeyCode;
        Enum.TryParse(newValue, true, out newKeyCode);
        Instance.HotkeyBindings[hotkey] = newKeyCode;
    }

    public static void UpdateRerollHotkey(string newValue)
        => UpdateHotkey(HotkeyEnum.RerollHotkey, newValue);

    public static void UpdateBuyXPHotkey(string newValue)
        => UpdateHotkey(HotkeyEnum.BuyXPHotkey, newValue);

    public static void UpdateSellChampionHotKey(string newValue)
        => UpdateHotkey(HotkeyEnum.SellChampionHotkey, newValue);


    public static string ValidateHotkey(HotkeyEnum hotkey, string currentValue)
    {
        if (string.IsNullOrEmpty(currentValue) || CheckDuplicateHotkeys(currentValue))
            return Instance.HotkeyBindings[hotkey].ToString();
        else
            return currentValue;
    }

    public static string ValidateRerollHotkey(string currentValue)
        => ValidateHotkey(HotkeyEnum.RerollHotkey, currentValue);

    public static string ValidateBuyXPHotkey(string currentValue)
        => ValidateHotkey(HotkeyEnum.BuyXPHotkey, currentValue);

    public static string ValidateSellChampionHotKey(string currentValue)
        => ValidateHotkey(HotkeyEnum.SellChampionHotkey, currentValue);
} 