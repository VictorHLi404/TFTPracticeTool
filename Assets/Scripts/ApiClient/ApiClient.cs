using UnityEngine.Networking;
using Cysharp.Threading.Tasks; // Required for UniTask
using Newtonsoft;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Collections.Generic;
public class ApiClient
{
    public static readonly string baseUrl = "http://localhost:5000";

    // public static async UniTask<T> GetRequest<T>(string path, Dictionary<string, string> parameters)
    // {

    // }

    public static async UniTask<decimal> TestFunc()
    {
        ChampionRequest champion = new ChampionRequest
        {
            ChampionName = ChampionEnum.Rhaast,
            Items = new List<AllItemsEnum>(),
            Level = 2
        };
        decimal average = await PostRequest<ChampionRequest, decimal>(
            "/Champion/ChampionWinrate",
            champion
        );
        Debug.Log("HOLY SHIT THE API ACTUALLY WORKS");
        Debug.Log(average);
        return average;
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
                jsonPayload = JsonConvert.SerializeObject(requestBody);
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

                    return JsonConvert.DeserializeObject<TResponse>(jsonResponse);
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