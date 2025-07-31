using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks; // Required for UniTask
using NUnit.Framework.Interfaces;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingResources : MonoBehaviour
{
    public static StartingResources Instance { get; set; }
    public List<Component> initialComponents;
    public List<Champion> initialChampions;
    public int initialLevel = 5;
    public int initialGold = 50;

    private decimal expectedPlacement;

    public TMP_Dropdown LevelDropdownSelection;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            initialGold = 50;
            initialLevel = 5;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Accessing starting resources from somewhere else.");
        }

    }

    public async void StartGame()
    {
        if (Instance == null)
        {
            Debug.LogError("Instance for StartingResources has not been set yet.");
        }
        SceneManager.LoadScene("TestScene");
        // Instance.initialComponents = RandomizationHelper.GenerateRandomComponents();

        // var initialTeamResults = await ApiClient.GetPopularTeamComp(Instance.initialLevel);

        // (Instance.expectedPlacement, Instance.initialChampions) = DtosHelper.DeserializeTeamResponse(initialTeamResults);
        // RandomizationHelper.DelevelTeam(initialChampions);


    }

    public void UpdateLevel(int index)
    {
        if (LevelDropdownSelection == null)
        {
            Debug.LogError("Cannot update the level dropdown when no TMP dropdown is associated with this current accessing instance in the inspector.");
            return;
        }
        var options = LevelDropdownSelection.options;
        var levelString = options[index].text;
        int level;
        if (!int.TryParse(levelString, out level))
        {
            Debug.LogError("A valid integer value was not passed from the dropdown menu.");
            return;
        }
        if (level < 5 || level > 10)
        {
            Debug.LogError("An invalid level was passed. Needs to be between 5-10.");
            return;
        }
        Instance.initialLevel = level;
        Debug.Log($"UPDATED DROPDOWN VALUE TO {level}");
        Debug.Log(index);
    }

    public void UpdateGold(string value)
    {
        int gold;
        if (!int.TryParse(value, out gold))
        {
            Debug.LogWarning("A valid integer value was not passed in.");
            return;
        }
        if (gold < 1 || gold > 99)
        {
            Debug.LogWarning("An invalid gold amount was passed. Needs to be between 5-10.");
            return;
        }
        Instance.initialGold = gold;
    }

    public static string ValidateGold(string currentValue)
    {
        int gold;
        if (!int.TryParse(currentValue, out gold))
        {
            return Instance.initialGold.ToString();
        }
        if (gold < 1 || gold > 99)
        {
            return Instance.initialGold.ToString();
        }
        return currentValue;
    }
}