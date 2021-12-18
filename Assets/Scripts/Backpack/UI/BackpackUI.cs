using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BackpackUI : MonoBehaviour
{
    private Canvas BackpackCanvas;
    private ItemUIPlace[] itemUIPlaces;

    public UnityEvent<ItemController> OnPullOut;

    private void Start()
    {
        BackpackCanvas = GetComponent<Canvas>();
        SetCanvasEnable(false);
        itemUIPlaces = GetComponentsInChildren<ItemUIPlace>();
        foreach(var places in itemUIPlaces)
        {
            places.OnPullOut.AddListener(OnPullOut.Invoke);
        }
    }

    public void SetCanvasEnable(bool enabled)
    {
        BackpackCanvas.enabled = enabled;
    }

    public void PutIn(ItemController item)
    {
        ItemUIPlace itemPlace = itemUIPlaces.FirstOrDefault(place => place.Type == item.Type);
        if (itemPlace == null)
        {
            Debug.LogErrorFormat("Не указано место для объекта типа {0}", item.Type);
            return;
        }
        itemPlace.AddItem(item);
    }
}
