using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Класс для управления UI частью рюкзака
/// </summary>
public class BackpackUI : MonoBehaviour
{
    private Canvas BackpackCanvas;
    private ItemUIPlace[] itemUIPlaces;

    public UnityEvent<ItemController> OnPullOut;

    private void Start()
    {
        // в начале canvas отключен
        BackpackCanvas = GetComponent<Canvas>();
        SetCanvasEnable(false);

        itemUIPlaces = GetComponentsInChildren<ItemUIPlace>();
        foreach(var places in itemUIPlaces)
        {
            // подписываемся на проброс события дальше, в BackpackController
            places.OnPullOut.AddListener(OnPullOut.Invoke);
        }
    }

    /// <summary>
    /// Включение / выключение Canvas
    /// </summary>
    public void SetCanvasEnable(bool enabled)
    {
        BackpackCanvas.enabled = enabled;
    }

    /// <summary>
    /// Добавление объекта в ui рюкзака
    /// </summary>
    public void PutIn(ItemController item)
    {
        // находим место, соответствующее типу
        ItemUIPlace itemPlace = itemUIPlaces.FirstOrDefault(place => place.Type == item.Type);
        if (itemPlace == null)
        {
            Debug.LogErrorFormat("Не указано место для объекта типа {0}", item.Type);
            return;
        }
        // добавляем объект
        itemPlace.AddItem(item);
    }
}
