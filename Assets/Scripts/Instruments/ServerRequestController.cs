using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum EventType { PutIn, PullOut };

public class ServerRequestController : MonoBehaviour
{
    public string ServerUrl = "https://devo4ka11.elysium.today/inventory/status";
    public string authKey = "auth";
    public string authValue = "BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6";
    public BackpackController backpackController;

    private void Awake()
    {
        backpackController.OnPutIn.AddListener(OnPutIn);
        backpackController.OnPullOut.AddListener(OnPullOut);
    }

    private void OnPutIn(ItemController item)
    {
        // ToDo: стереть
        Debug.Log("PUT IN");
        StartCoroutine(SendRequest(item.Id, "PutIn"));
    }

    private void OnPullOut(ItemController item)
    {
        // ToDo: стереть
        Debug.Log("PULL OUT");
        StartCoroutine(SendRequest(item.Id, "PullOut"));
    }

    private IEnumerator SendRequest(string itemId, string eventType)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemId", itemId);
        form.AddField("event", eventType);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(ServerUrl, itemId))
        {
            webRequest.SetRequestHeader(authKey, authValue);
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}