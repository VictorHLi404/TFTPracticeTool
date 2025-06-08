using TMPro;
using UnityEngine;

/// <summary>
/// A class that primarily handles read displays of the current round stats (round, stage, time, etc).
/// </summary>
public class RoundInformationManager : MonoBehaviour
{
    private Player player;
    private GameObject roundDisplayField;
    private GameObject timeDisplayField;
    private GameObject timeBar;

    private float currentTime;

    public void Awake()
    {
        this.roundDisplayField = transform.Find("RoundDisplayField").gameObject;
        this.timeDisplayField = transform.Find("TimeDisplayField").gameObject;
        this.timeBar = transform.Find("TimeBarFill").gameObject;
        this.player = new Player(6, 0, 200, 30, 4, 2, DatabaseAPI.getLevelMapping());
        Debug.Log("GET THAT HOE!");
        currentTime = player.time;
        InitializeDisplays();
    }

    public void Initialize(Player newPlayer)
    {
        this.player = newPlayer;
        InitializeDisplays();
    }


    public void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimeDisplays();
        }

    }

    public void InitializeDisplays()
    {
        roundDisplayField.GetComponent<TextMeshPro>().text = $"{player.stage}-{player.round}";
        timeDisplayField.GetComponent<TextMeshPro>().text = $"{player.time}";
        float TimeBarMaxLength = transform.Find("TimeBarBackground").transform.localScale.x;
        float TimeBarDefaultX = transform.Find("TimeBarBackground").transform.localPosition.x - (TimeBarMaxLength / 2);
        float newTimeBarLength = TimeBarMaxLength;
        timeBar.transform.localScale = new Vector3(newTimeBarLength, timeBar.transform.localScale.y, timeBar.transform.localScale.z);
        timeBar.transform.localPosition = new Vector3(newTimeBarLength / 2 + TimeBarDefaultX, timeBar.transform.localPosition.y, timeBar.transform.localPosition.z);
    }

    public void UpdateTimeDisplays()
    {
        if (currentTime > 0)
        {
            int newTime = (int)currentTime + 1;
            timeDisplayField.GetComponent<TextMeshPro>().text = $"{newTime}";
        }
        else
        {
            timeDisplayField.GetComponent<TextMeshPro>().text = $"0";
        }

        float TimeBarMaxLength = transform.Find("TimeBarBackground").transform.localScale.x;
        float TimeBarDefaultX = transform.Find("TimeBarBackground").transform.localPosition.x + (TimeBarMaxLength / 2);
        float newTimeBarLength = TimeBarMaxLength * (currentTime / player.time);
        timeBar.transform.localScale = new Vector3(newTimeBarLength, timeBar.transform.localScale.y, timeBar.transform.localScale.z);
        timeBar.transform.localPosition = new Vector3(TimeBarDefaultX - (newTimeBarLength / 2) , timeBar.transform.localPosition.y, timeBar.transform.localPosition.z);
    }

}