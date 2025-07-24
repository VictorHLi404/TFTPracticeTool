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

    public static async UniTask<ChampionResponse> GetChampionWinrate(Champion champion)
    {
        ChampionEnum championEnum;
        if (!Enum.TryParse(ProcessingHelper.CleanChampionName(champion.unitName), out championEnum))
            Debug.LogError($"Could not parse the champion name {champion.unitName} properly.");

        List<AllItemsEnum> allItems = new List<AllItemsEnum>();

        foreach (var item in champion.GetItems())
        {
            allItems.Add(item.GetAllItemsEnum());
        }

        ChampionRequest request = new ChampionRequest
        {
            ChampionName = championEnum,
            Items = allItems,
            Level = champion.starLevel
        };

        return await PostRequest<ChampionRequest, ChampionResponse>(
            ChampionWinratePath,
            request
        );
    }

    public static async UniTask<ChampionResponse> TestChampionWinrate()
    {

        Champion testChampion = new Champion(2,
            new UnitData
            {
                databaseID = 1,
                unitName = "Rhaast",
                cost = 2,
                unitTraits = new List<string> { "Divinicorp", "Vanguard" },
                shopIconName = "",
                championIconName = ""
            }
        );
        Debug.Log("TEST THE API FUNC FOR CHAMPION WINRATE");
        ChampionResponse response = await GetChampionWinrate(testChampion);
        Debug.Log("REQUEST SUCCESSFULLY COMPLETED");
        Debug.Log(response);
        Debug.Log(response.ChampionName);
        Debug.Log(response.AveragePlacement);

        return response;
    }

    public static async UniTask<List<ChampionResponse>> GetChampionAlternativeBuilds(Champion champion, List<List<AllItemsEnum>> alternativeItems)
    {
        ChampionEnum championEnum;
        if (!Enum.TryParse(ProcessingHelper.CleanChampionName(champion.unitName), out championEnum))
            Debug.LogError($"Could not parse the champion name {champion.unitName} properly.");

        List<AllItemsEnum> allItems = new List<AllItemsEnum>();

        foreach (var item in champion.GetItems())
        {
            allItems.Add(item.GetAllItemsEnum());
        }

        var championRequest = new ChampionRequest
        {
            ChampionName = championEnum,
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

    public static async UniTask<List<ChampionResponse>> TestChampionAlternativeBuilds()
    {
        Champion testChampion = new Champion(2,
            new UnitData
            {
                databaseID = 53,
                unitName = "Aurora",
                cost = 5,
                unitTraits = new List<string> { "Anima Squad", "Dynamo" },
                shopIconName = "",
                championIconName = ""
            }
        );
        List<List<AllItemsEnum>> alternativeItemSets = new List<List<AllItemsEnum>>
        {
            new List<AllItemsEnum> { AllItemsEnum.StrikersFlail, AllItemsEnum.JeweledGauntlet, AllItemsEnum.GiantSlayer, AllItemsEnum.SpearofShojin },
            new List<AllItemsEnum> { AllItemsEnum.BlueBuff, AllItemsEnum.NeedlesslyLargeRod, AllItemsEnum.JeweledGauntlet, AllItemsEnum.TearoftheGoddess },
            new List<AllItemsEnum> { AllItemsEnum.RecurveBow, AllItemsEnum.BlueBuff, AllItemsEnum.JeweledGauntlet, AllItemsEnum.SpearofShojin },
            new List<AllItemsEnum> { AllItemsEnum.RabadonsDeathcap, AllItemsEnum.JeweledGauntlet, AllItemsEnum.NeedlesslyLargeRod, AllItemsEnum.ThiefsGloves },
            new List<AllItemsEnum> { AllItemsEnum.VoidStaff, AllItemsEnum.BFSword, AllItemsEnum.GuinsoosRageblade, AllItemsEnum.SpearofShojin }
        };

        Debug.Log("TEST THE API FUNC FOR CHAMPION ALT BUILDS");
        List<ChampionResponse> response = await ApiClient.GetChampionAlternativeBuilds(testChampion, alternativeItemSets);
        Debug.Log("REQUEST SUCCESSFULLY COMPLETED");
        Debug.Log(response);

        foreach (var responseObject in response)
        {
            Debug.Log(responseObject.AveragePlacement);
        }

        return response;
    }

    public static async UniTask<TResponse?> PostRequest<TRequest, TResponse>(
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

                    return JsonConvert.DeserializeObject<TResponse>(jsonResponse, new JsonSerializerSettings { Converters = {new StringEnumConverter() }});
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An unexpected error occurred during POST to {fullUrl}: {ex.Message}");
            throw;
        }
    }

    private static async UniTask<string> MakeUniTaskGetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.timeout = 10;

            // UniTask provides an extension method to await UnityWebRequest directly
            await webRequest.SendWebRequest();

            // No need for while(!isDone) loop, UniTask handles it.
            // If the request is cancelled while awaiting, UniTask will throw OperationCanceledException.

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                throw new System.Exception("Network Error: " + webRequest.error);
            }
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                throw new System.Exception($"HTTP Error: {webRequest.error} (Code: {webRequest.responseCode})");
            }
            else
            {
                return webRequest.downloadHandler.text;
            }
        }
    }

}