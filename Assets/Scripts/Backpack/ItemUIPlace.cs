using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUIPlace : MonoBehaviour, IPointerExitHandler
{
    public ItemType Type;
    private Image icon;
    private ItemController savedItem;

    public UnityEvent<ItemController> OnPullOut;

    private void Start()
    {
        icon = GetComponentInChildren<Image>();
        icon.enabled = false;
    }

    public void AddItem(ItemController item)
    {
        icon.enabled = true;
        savedItem = item;
    }

    public void RemoveItem()
    {
        icon.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
        {
            RemoveItem();
            OnPullOut?.Invoke(savedItem);
        }
    }
}
