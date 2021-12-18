using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Место для объектов определенного типа на UI рюкзака
/// </summary>
public class ItemUIPlace : MonoBehaviour, IPointerExitHandler
{
    [Tooltip("Тип объекта, для которого предназначено место")]
    public ItemType Type;

    // иконка объекта
    private Image icon;
    // ссылка на объект в этом месте
    private ItemController savedItem;

    public UnityEvent<ItemController> OnPullOut;

    // в начале в ячейке пусто
    private void Start()
    {
        icon = GetComponentInChildren<Image>();
        icon.enabled = false;
    }

    /// <summary>
    /// Добавление объекта - включаем иконку
    /// </summary>
    public void AddItem(ItemController item)
    {
        icon.enabled = true;
        savedItem = item;
    }

    /// <summary>
    /// Удаление объекта - выключаем иконку
    /// </summary>
    public void RemoveItem()
    {
        icon.enabled = false;
    }

    // проверка, что ЛКМ была отпущена в пределах объекта в UI рюкзака
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
        {
            RemoveItem();
            // запускаем процесс доставания объекта из рюкзака
            OnPullOut?.Invoke(savedItem);
        }
    }
}
