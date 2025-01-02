using System;
using UnityEngine;
using UnityEngine.UI;
public class DropdownManager : MonoBehaviour
{
    public Dropdown LevelSelector;
    // Add something for gold selector
    // Add something for time selector
    public Dropdown StageSelector;
    public Dropdown RoundSelector;

    void Start()
    {
        // Listeners for each dropdown
        LevelSelector.onValueChanged.AddListener(OnLevelChanged);
        RoundSelector.onValueChanged.AddListener(OnRoundChanged);
        StageSelector.onValueChanged.AddListener(OnStageChanged);
    }

    void OnLevelChanged(int value)
    {
        Console.WriteLine(value);
    }

    void OnRoundChanged(int value)
    {
        Console.WriteLine(value);
    }

    void OnStageChanged(int value)
    {
        Console.WriteLine(value);
    }

}