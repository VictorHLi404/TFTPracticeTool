using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; set; }

    public Dictionary<HotkeyEnum, string> HotkeyBindings = new Dictionary<HotkeyEnum, string>
    {
        {HotkeyEnum.BuyXPHotkey, "B"},
        {HotkeyEnum.RerollHotkey, "R"},
        {HotkeyEnum.SellChampionHotkey, "E"}
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
        foreach (var existingHotkey in Instance.HotkeyBindings.Values)
        {
            if (newValue.Equals(existingHotkey))
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
        Instance.HotkeyBindings[hotkey] = newValue;
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
            return Instance.HotkeyBindings[hotkey];
        else
            return currentValue;
    }

    public static string ValidateRerollHotkey(string currentValue)
        => ValidateHotkey(HotkeyEnum.RerollHotkey, currentValue);

    public static string ValidateBuyXPHotkey(string currentValue)
        => ValidateHotkey(HotkeyEnum.BuyXPHotkey, currentValue);

    public static string ValidateSellChampionHotKey(string currentValue)
        => ValidateHotkey(HotkeyEnum.SellChampionHotkey, currentValue);

    public static string GetBuyXPHotkey()
    {
        if (!Instance.HotkeyBindings.ContainsKey(HotkeyEnum.BuyXPHotkey))
            Debug.LogError("Buy XP Hotkey was not set properly.");
        return Instance.HotkeyBindings[HotkeyEnum.BuyXPHotkey];
    }
    public static string GetRerollHotkey()
    {
        if (!Instance.HotkeyBindings.ContainsKey(HotkeyEnum.RerollHotkey))
        {
            Debug.LogError("Reroll Hotkey was not set properly in Instance.HotkeyBindings dictionary.");
        }
        return Instance.HotkeyBindings[HotkeyEnum.RerollHotkey];
    }
    public static string GetSellChampionHotkey()
    {
        if (!Instance.HotkeyBindings.ContainsKey(HotkeyEnum.SellChampionHotkey))
        {
            Debug.LogError("Sell Champion Hotkey was not set properly in Instance.HotkeyBindings dictionary.");
        }
        return Instance.HotkeyBindings[HotkeyEnum.SellChampionHotkey];
    }
} 