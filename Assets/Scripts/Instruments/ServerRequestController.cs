using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Класс для отправки запросов на сервер
/// </summary>
public class ServerRequestController : MonoBehaviour
{
    [Tooltip("Ссылка на сервер для отправки запросов")]
    public string ServerUrl = "https://devo4ka11.elysium.today/inventory/status";
    [Tooltip("Имя ключа авторизации")]
    public string authKey = "auth";
    [Tooltip("Значение ключа авторизации")]
    public string authValue = "BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6";

    [Tooltip("Контроллер рюкзака, на события которого подписываемся")]
    public BackpackController backpackController;

    // типы событий - добавление и доставание из рюкзака
    public enum EventType { PutIn, PullOut };

    private void Awake()
    {
        backpackController.OnPutIn.AddListener(OnPutIn);
        backpackController.OnPullOut.AddListener(OnPullOut);
    }

    /// <summary>
    /// Отправка запроса с события о добавлении объекта в рюкзак
    /// </summary>
    private void OnPutIn(ItemController item)
    {
        // ToDo: стереть
        Debug.Log("PUT IN");
        // ToDo: проверить
        StartCoroutine(SendRequest(item.Id, EventType.PutIn.ToString()));
    }

    /// <summary>
    /// Отправка запроса с события о доставании объекта из рюкзака
    /// </summary>
    private void OnPullOut(ItemController item)
    {
        // ToDo: стереть
        Debug.Log("PULL OUT");
        // ToDo: проверить
        StartCoroutine(SendRequest(item.Id, EventType.PullOut.ToString()));
    }
    
    /// <summary>
    /// Отправка запроса на сервер
    /// </summary>
    /// <param name="itemId">Id объекта</param>
    /// <param name="eventType">Тип события</param>
    /// <returns></returns>
    private IEnumerator SendRequest(string itemId, string eventType)
    {
        // добавляем параметры на форму
        WWWForm form = new WWWForm();
        form.AddField("itemId", itemId);
        form.AddField("event", eventType);
        // создаем post request
        using (UnityWebRequest webRequest = UnityWebRequest.Post(ServerUrl, itemId))
        {
            // добавляем ключ авторизации
            webRequest.SetRequestHeader(authKey, authValue);
            // отправляем и ждем завершения
            yield return webRequest.SendWebRequest();

            // проверяем на ошибку
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
            }
        }
    }
}