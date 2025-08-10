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
    public TeamResponse initialTeamStatistics;
    public int initialLevel = 5;
    public int initialGold = 50;
    public int initialTime = 30;
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

        Instance.initialComponents = RandomizationHelper.GenerateRandomComponents();

        Instance.initialTeamStatistics = await ApiClient.GetPopularTeamComp(Instance.initialLevel);

        (Instance.expectedPlacement, Instance.initialChampions) = DtosHelper.DeserializeTeamResponse(Instance.initialTeamStatistics);
        RandomizationHelper.DelevelTeam(Instance.initialChampions);

        SceneManager.LoadScene("TestScene");

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

    public void UpdateTime(string value)
    {
        int time;
        if (!int.TryParse(value, out time))
        {
            Debug.LogWarning("A valid integer value was not passed in.");
            return;
        }
        if (time < 15 || time > 99)
        {
            Debug.LogWarning($"An invalid time amount was passed. Needs to be between 15-99. : {time}");
            return;
        }
        Instance.initialTime = time;
    }

    public static string ValidateTime(string currentValue)
    {
        int time;
        if (!int.TryParse(currentValue, out time))
        {
            return Instance.initialTime.ToString();
        }
        if (time < 15 || time > 99)
        {
            return Instance.initialTime.ToString();
        }
        return currentValue;
    }
}