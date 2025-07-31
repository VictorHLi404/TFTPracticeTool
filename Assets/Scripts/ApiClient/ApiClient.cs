using UnityEngine.Networking;
using Cysharp.Threading.Tasks; // Required for UniTask
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
public class ApiClient
{
    public static readonly string baseUrl = "http://localhost:5000";
    public static readonly string ChampionWinratePath = "/Champion/ChampionWinrate";
    public static readonly string ChampionItemsPath = "/Champion/ChampionItems";
    public static readonly string TeamWinratePath = "/Team/TeamWinrate";
    public static readonly string TeamAlternativeCompsPath = "/Team/TeamAlternativeComps";
    public static readonly string TeamPopularTeamComp = "/Team/PopularTeamComp";

    public static async UniTask<ChampionResponse> GetChampionWinrate(Champion champion)
    {
        List<AllItemsEnum> allItems = new List<AllItemsEnum>();

        foreach (var item in champion.GetItems())
        {
            allItems.Add(item.GetAllItemsEnum());
        }

        ChampionRequest request = new ChampionRequest
        {
            ChampionName = champion.UnitName ?? throw new Exception("champion is not set to a value."),
            Items = allItems,
            Level = champion.starLevel
        };

        return await PostRequest<ChampionRequest, ChampionResponse>(
            ChampionWinratePath,
            request
        );
    }

    public static async UniTask<List<ChampionResponse>> GetChampionAlternativeBuilds(Champion champion, List<List<AllItemsEnum>> alternativeItems)
    {
        List<AllItemsEnum> allItems = new List<AllItemsEnum>();

        foreach (var item in champion.GetItems())
        {
            allItems.Add(item.GetAllItemsEnum());
        }

        var championRequest = new ChampionRequest
        {
            ChampionName = champion.UnitName ?? throw new Exception("champion is not set to a value."),
            Items = allItems,
            Level = champion.starLevel
        };

        var request = new ChampionItemStatisticsRequest
        {
            MainChampion = championRequest,
            PossibleItemSets = alternativeItems
        };

        return await PostRequest<ChampionItemStatisticsRequest, List<ChampionResponse>>(
            ChampionItemsPath,
            request
        );
    }

    public static async UniTask<TeamResponse> GetPopularTeamComp(int level)
    {
        var request = new PopularTeamCompRequest
        {
            Level = level
        };

        return await PostRequest<PopularTeamCompRequest, TeamResponse>(
            TeamPopularTeamComp,
            request
        );
    }

    public static async UniTask<TeamResponse> GetTeamWinrate(List<Champion> champions)
    {
        List<ChampionRequest> championRequests = new List<ChampionRequest>();
        foreach (var champion in champions)
        {
            championRequests.Add(new ChampionRequest
            {
                ChampionName = champion.UnitName ?? throw new Exception("champion is not set to a value."),
                Items = new List<AllItemsEnum>(),
                Level = champion.starLevel
            });
        }

        var request = new TeamRequest
        {
            Level = championRequests.Count,
            Champions = championRequests
        };

        return await PostRequest<TeamRequest, TeamResponse>(
            TeamWinratePath,
            request
        );
    }

    public static async UniTask<List<TeamResponse>> GetTeamAlternativeComps(List<Champion> team, List<Champion> benchChampions)
    {
        List<ChampionRequest> teamChampionRequests = new List<ChampionRequest>();
        foreach (var champion in team)
        {
            teamChampionRequests.Add(new ChampionRequest
            {
                ChampionName = champion.UnitName ?? throw new Exception("champion is not set to a value."),
                Items = new List<AllItemsEnum>(),
                Level = champion.starLevel
            });
        }

        var teamRequest = new TeamRequest
        {
            Level = teamChampionRequests.Count,
            Champions = teamChampionRequests
        };

        List<ChampionRequest> alternateChampions = new List<ChampionRequest>();

        foreach (var champion in benchChampions)
        {
            alternateChampions.Add(new ChampionRequest
            {
                ChampionName = champion.UnitName ?? throw new Exception("champion is not set to a value."),
                Items = new List<AllItemsEnum>(),
                Level = champion.starLevel
            });
        }

        var request = new TeamAlternativeStatisticsRequest
        {
            Team = teamRequest,
            AlternativeChampions = alternateChampions
        };

        return await PostRequest<TeamAlternativeStatisticsRequest, List<TeamResponse>>(
            TeamAlternativeCompsPath,
            request
        );

    }

    public static async UniTask<TResponse> PostRequest<TRequest, TResponse>(
        string path,
        TRequest requestBody)
        where TRequest : class
    {
        if (string.IsNullOrEmpty(path))
            Debug.LogError("Invalid path string was provided.");

        string fullUrl = $"{baseUrl}{path}";
        string jsonPayload = string.Empty;

        try
        {
            if (requestBody != null)
                jsonPayload = JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings { Converters = { new StringEnumConverter() } });
            else
                Debug.LogWarning($"No JSON body was provided for the post request to {fullUrl}");

            using (UnityWebRequest webRequest = new UnityWebRequest(fullUrl, UnityWebRequest.kHttpVerbPOST))
            {
                if (!string.IsNullOrEmpty(jsonPayload))
                {
                    byte[] body = Encoding.UTF8.GetBytes(jsonPayload);
                    webRequest.uploadHandler = new UploadHandlerRaw(body);
                }
                else
                    webRequest.uploadHandler = new UploadHandlerRaw(new byte[0]);

                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.timeout = 20;

                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    throw new Exception($"Network Error for POST {path}: {webRequest.error}");
                }
                else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    throw new Exception($"HTTP Error for POST {path}: {webRequest.error} (Code: {webRequest.responseCode}). Response: {webRequest.downloadHandler.text}");
                }
                else // Success
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    Debug.Log($"Received POST response from {path}: {jsonResponse}");

                    // 4. Deserialize the response
                    if (string.IsNullOrEmpty(jsonResponse))
                    {
                        Debug.LogWarning($"ApiClient.PostRequest: Received empty response for {path}. Returning default({typeof(TResponse).Name}).");
                        return default(TResponse); // Return default for TResponse if response is empty
                    }

                    return JsonConvert.DeserializeObject<TResponse>(jsonResponse, new JsonSerializerSettings { Converters = { new StringEnumConverter() } });
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An unexpected error occurred during POST to {fullUrl}: {ex.Message}");
            throw;
        }
    }

    // public static async UniTask<ChampionResponse> TestChampionWinrate()
    // {

    //     Champion testChampion = new Champion(2,
    //         new UnitData
    //         {
    //             DatabaseID = 1,
    //             UnitName = "Rhaast",
    //             Cost = 2,
    //             UnitTraits = new List<string> { "Divinicorp", "Vanguard" },
    //             ShopIconName = "",
    //             ChampionIconName = ""
    //         }
    //     );
    //     Debug.Log("TEST THE API FUNC FOR CHAMPION WINRATE");
    //     ChampionResponse response = await GetChampionWinrate(testChampion);
    //     Debug.Log("REQUEST SUCCESSFULLY COMPLETED");
    //     Debug.Log(response);
    //     Debug.Log(response.ChampionName);
    //     Debug.Log(response.AveragePlacement);

    //     return response;
    // }

    // public static async UniTask<List<ChampionResponse>> TestChampionAlternativeBuilds()
    // {
    //     Champion testChampion = new Champion(2,
    //         new UnitData
    //         {
    //             DatabaseID = 53,
    //             UnitName = "Aurora",
    //             Cost = 5,
    //             UnitTraits = new List<string> { "Anima Squad", "Dynamo" },
    //             ShopIconName = "",
    //             ChampionIconName = ""
    //         }
    //     );
    //     List<List<AllItemsEnum>> alternativeItemSets = new List<List<AllItemsEnum>>
    //     {
    //         new List<AllItemsEnum> { AllItemsEnum.StrikersFlail, AllItemsEnum.JeweledGauntlet, AllItemsEnum.GiantSlayer, AllItemsEnum.SpearofShojin },
    //         new List<AllItemsEnum> { AllItemsEnum.BlueBuff, AllItemsEnum.NeedlesslyLargeRod, AllItemsEnum.JeweledGauntlet, AllItemsEnum.TearoftheGoddess },
    //         new List<AllItemsEnum> { AllItemsEnum.RecurveBow, AllItemsEnum.BlueBuff, AllItemsEnum.JeweledGauntlet, AllItemsEnum.SpearofShojin },
    //         new List<AllItemsEnum> { AllItemsEnum.RabadonsDeathcap, AllItemsEnum.JeweledGauntlet, AllItemsEnum.NeedlesslyLargeRod, AllItemsEnum.ThiefsGloves },
    //         new List<AllItemsEnum> { AllItemsEnum.VoidStaff, AllItemsEnum.BFSword, AllItemsEnum.GuinsoosRageblade, AllItemsEnum.SpearofShojin }
    //     };

    //     Debug.Log("TEST THE API FUNC FOR CHAMPION ALT BUILDS");
    //     List<ChampionResponse> response = await ApiClient.GetChampionAlternativeBuilds(testChampion, alternativeItemSets);
    //     Debug.Log("REQUEST SUCCESSFULLY COMPLETED");
    //     Debug.Log(response);

    //     foreach (var responseObject in response)
    //     {
    //         Debug.Log(responseObject.AveragePlacement);
    //     }

    //     return response;
    // }

    // public static async UniTask<TeamResponse> TestTeamWinrate()
    // {
    //     List<Champion> champions = new List<Champion>
    //     {
    //         new Champion(2, new UnitData { UnitName = "Brand", Cost = 4 }),
    //         new Champion(2, new UnitData { UnitName = "Neeko", Cost = 4 }),
    //         new Champion(1, new UnitData { UnitName = "Samira", Cost = 5 }),
    //         new Champion(1, new UnitData { UnitName = "Ziggs", Cost = 4 }),
    //         new Champion(1, new UnitData { UnitName = "Kobuko", Cost = 5 }),
    //         new Champion(2, new UnitData { UnitName = "Rengar", Cost = 3 }),
    //         new Champion(2, new UnitData { UnitName = "Ekko", Cost = 2 }),
    //         new Champion(2, new UnitData { UnitName = "DrMundo", Cost = 1 }),
    //         new Champion(2, new UnitData { UnitName = "Zyra", Cost = 1 })
    //     };
    //     Debug.Log("TEST THE API FUNC FOR TEAM WINRATE DATA");
    //     TeamResponse response = await GetTeamWinrate(champions);
    //     Debug.Log("REQUEST SUCCESSFULL COMPLETED");
    //     Debug.Log(response.AveragePlacement);
    //     foreach (var champion in response.Champions)
    //     {
    //         Debug.Log(champion.ChampionName);
    //     }
    //     return response;
    // }
}