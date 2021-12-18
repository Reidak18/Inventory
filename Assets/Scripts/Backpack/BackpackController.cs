using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Основной класс управления рюкзаком
/// </summary>
public class BackpackController : MonoBehaviour
{
    [Tooltip("Контроллер визуального отображения объектов в рюкзаке")]
    public BackpackUI BackpackCanvas;

    // объект, который может быть положен в рюкзак, если мы его отпустим
    private ItemController itemToPut;
    // места на рюкзаке под разные типы объектов
    private ItemBackpackPlace[] itemBackpackPlaces;

    // событие добавления в рюкзак
    public UnityEvent<ItemController> OnPutIn;
    // событие доставания из рюкзака 
    public UnityEvent<ItemController> OnPullOut;

    private void Start()
    {
        BackpackCanvas.OnPullOut.AddListener(PullOut);
        itemBackpackPlaces = GetComponentsInChildren<ItemBackpackPlace>();
    }

    private void OnMouseDown()
    {
        BackpackCanvas.SetCanvasEnable(true);
    }

    private void OnMouseUp()
    {
        BackpackCanvas.SetCanvasEnable(false);
    }

    // при пересечении с рюкзаком, запоминаем объект, как готовый к добавлению
    private void OnTriggerEnter(Collider other)
    {
        ItemController item = other.GetComponent<ItemController>();
        if (item != null)
        {
            itemToPut = item;
        }
    }

    // проверка, не пора ли добавить 
    private void Update()
    {
        if (itemToPut != null && !itemToPut.isMoving)
        {
            TakeItem(itemToPut);
            itemToPut = null;
        }
    }

    /// <summary>
    /// Добавление объекта в рюкзак
    /// </summary>
    private void TakeItem(ItemController item)
    {
        // находим место, соответствующее типу объекта 
        ItemBackpackPlace itemPlace = itemBackpackPlaces.FirstOrDefault(place => place.Type == item.Type);
        if (itemPlace == null)
        {
            Debug.LogErrorFormat("Не указано место для объекта типа {0}", item.Type);
            return;
        }
        // переносим в это место
        item.transform.SetParent(itemPlace.transform);
        // отключаем лишние функции и переносим на рюкзак
        item.PutInBackpack();
        // добавляем на канвас
        BackpackCanvas.PutIn(item);

        OnPutIn?.Invoke(item);
    }

    /// <summary>
    /// Доставание объекта из рюкзака
    /// </summary>
    private void PullOut(ItemController item)
    {
        // находим место, соответствующее типу объекта 
        ItemBackpackPlace itemPlace = itemBackpackPlaces.FirstOrDefault(place => place.Type == item.Type);
        if (itemPlace == null)
        {
            Debug.LogErrorFormat("Не найдено место для объекта типа {0}", item.Type);
            return;
        }
        // восстанавливаем функции и отделяем от рюкзака
        item.PullOutBackpack();
        OnPullOut?.Invoke(item);
    }
}
