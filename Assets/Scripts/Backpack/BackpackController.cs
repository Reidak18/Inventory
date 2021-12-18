using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BackpackController : MonoBehaviour
{
    public BackpackUI BackpackCanvas;

    private ItemController itemToPut;
    private ItemBackpackPlace[] itemBackpackPlaces;

    public UnityEvent<ItemController> OnPutIn;
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

    private void OnTriggerEnter(Collider other)
    {
        ItemController item = other.GetComponent<ItemController>();
        if (item != null)
        {
            itemToPut = item;
        }
    }

    private void Update()
    {
        CheckItemToPut();
    }

    private void CheckItemToPut()
    {
        if (itemToPut != null && !itemToPut.isMoving)
        {
            TakeItem(itemToPut);
            itemToPut = null;
        }
    }

    private void TakeItem(ItemController item)
    {
        ItemBackpackPlace itemPlace = itemBackpackPlaces.FirstOrDefault(place => place.Type == item.Type);
        if (itemPlace == null)
        {
            Debug.LogErrorFormat("Не указано место для объекта типа {0}", item.Type);
            return;
        }
        item.transform.SetParent(itemPlace.transform);
        item.PutInBackpack();
        BackpackCanvas.PutIn(item);
        OnPutIn?.Invoke(item);
    }

    private void PullOut(ItemController item)
    {
        ItemBackpackPlace itemPlace = itemBackpackPlaces.FirstOrDefault(place => place.Type == item.Type);
        if (itemPlace == null)
        {
            Debug.LogErrorFormat("Не найдено место для объекта типа {0}", item.Type);
            return;
        }
        item.PullOutBackpack();
        OnPullOut?.Invoke(item);
    }
}
