using System.Collections.Generic;
using UnityEngine;

public class PostGameModal : MonoBehaviour
{
    private bool isLoading;

    [Header("Reference Fields")]
    public GameObject FirstChampion;
    public GameObject SecondChampion;
    public GameObject Team;

    public void Initialize(List<ChampionEntity> teamChampions,
                            List<(UnitData unit, int occurences)> shopChampions,
                            List<Component> initialChampions)
    {
        Debug.Log("GOT HERE!");
    }
}