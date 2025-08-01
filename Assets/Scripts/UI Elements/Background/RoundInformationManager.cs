using TMPro;
using UnityEngine;

/// <summary>
/// A class that primarily handles read displays of the current round stats (round, stage, time, etc).
/// </summary>
public class RoundInformationManager : MonoBehaviour
{
    private Player player;
    private GameObject RoundDisplayField;
    private GameObject TimeDisplayField;
    private GameObject TimeBar;
    public GameObject ShopUIReference;
    public GameObject BoardReference;
    public GameObject UIBlocker;
    public GameObject PostGameModal;
    private float CurrentTime;
    private bool HasGameEnded { get; set; }

    public void Start()
    {
        this.RoundDisplayField = transform.Find("RoundDisplayField").gameObject;
        this.TimeDisplayField = transform.Find("TimeDisplayField").gameObject;
        this.TimeBar = transform.Find("TimeBarFill").gameObject;
        this.player = ShopUIReference.GetComponent<ShopUI>().GetPlayer();
        CurrentTime = player.time;
        HasGameEnded = false;
        InitializeDisplays();
    }

    public void Initialize(Player newPlayer)
    {
        this.player = newPlayer;
        InitializeDisplays();
    }


    public void Update()
    {
        if (CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;
            UpdateTimeDisplays();
        }
        else
        {
            if (!HasGameEnded)
            {
                // HasGameEnded = true;
                // EndGame();
            }
        }

    }

    public void InitializeDisplays()
    {
        RoundDisplayField.GetComponent<TextMeshPro>().text = $"{player.stage}-{player.round}";
        TimeDisplayField.GetComponent<TextMeshPro>().text = $"{player.time}";
        float TimeBarMaxLength = transform.Find("TimeBarBackground").transform.localScale.x;
        float TimeBarDefaultX = transform.Find("TimeBarBackground").transform.localPosition.x - (TimeBarMaxLength / 2);
        float newTimeBarLength = TimeBarMaxLength;
        TimeBar.transform.localScale = new Vector3(newTimeBarLength, TimeBar.transform.localScale.y, TimeBar.transform.localScale.z);
        TimeBar.transform.localPosition = new Vector3(newTimeBarLength / 2 + TimeBarDefaultX, TimeBar.transform.localPosition.y, TimeBar.transform.localPosition.z);
    }

    public void UpdateTimeDisplays()
    {
        if (CurrentTime > 0)
        {
            int newTime = (int)CurrentTime + 1;
            TimeDisplayField.GetComponent<TextMeshPro>().text = $"{newTime}";
        }
        else
        {
            TimeDisplayField.GetComponent<TextMeshPro>().text = $"0";
        }

        float TimeBarMaxLength = transform.Find("TimeBarBackground").transform.localScale.x;
        float TimeBarDefaultX = transform.Find("TimeBarBackground").transform.localPosition.x + (TimeBarMaxLength / 2);
        float newTimeBarLength = TimeBarMaxLength * (CurrentTime / player.time);
        TimeBar.transform.localScale = new Vector3(newTimeBarLength, TimeBar.transform.localScale.y, TimeBar.transform.localScale.z);
        TimeBar.transform.localPosition = new Vector3(TimeBarDefaultX - (newTimeBarLength / 2), TimeBar.transform.localPosition.y, TimeBar.transform.localPosition.z);
    }

    public void EndGame()
    {
        var board = BoardReference.GetComponent<HexGridManager>();
        if (board == null)
            Debug.LogError("Incorrectly assigned board in round information manager.");
        var teamChampions = board.GetChampionEntities();
        var shopUI = ShopUIReference.GetComponent<ShopUI>();
        if (shopUI == null)
            Debug.LogError("Incorrectly assigned shop in round information manager.");
        var championOccurences = shopUI.GetChampionOccurrences();


    }

}