using TMPro;
using UnityEngine;

public class BoardCountDisplay : MonoBehaviour
{
    public TMP_Text textField;
    public SpriteRenderer IconReference;
    public GameObject BackgroundReference;
    public GameObject BoardReference;

    private int previousCount = -1;

    public void LateUpdate()
    {
        (var currentChampionCount, var maxChampionCount) = BoardReference.GetComponent<HexGridManager>().GetDisplayInformation();
        if (previousCount == -1)
        {
            previousCount = currentChampionCount;
            UpdateDisplay(currentChampionCount, maxChampionCount);
            return;
        }
        if (previousCount == currentChampionCount)
            return;
        else
        {
            previousCount = currentChampionCount;
            UpdateDisplay(currentChampionCount, maxChampionCount);
        }
        
    }

    public void UpdateDisplay(int championsOnBoard, int maxChampionsOnBoard)
    {
        Debug.Log(championsOnBoard);
        Debug.Log(maxChampionsOnBoard);
        textField.text = $"{championsOnBoard} / {maxChampionsOnBoard}";
        if (championsOnBoard == maxChampionsOnBoard)
        {
            textField.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            string filePath = $"CustomArtAssets/FilledBoardIcon";
            IconReference.sprite = Resources.Load<Sprite>(filePath);
            BackgroundReference.SetActive(false);
        }
        else
        {
            textField.color = new Color(0.24f, 0.44f, 0.71f, 1f);
            string filePath = $"CustomArtAssets/UnfilledBoardIcon";
            IconReference.sprite = Resources.Load<Sprite>(filePath);
            BackgroundReference.SetActive(true);
        }
    }
}