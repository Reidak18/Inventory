using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoveController : MonoBehaviour
{
    [Tooltip("Основная камера")]
    public Camera GameCamera;
    [Tooltip("Высота, на которую приподнимается объект при нажатии")]
    public float StartHeight;
    [Tooltip("Шаг при изменении высоты")]
    public float HeightStep;

    // текущий объект
    private ItemController currentItem;
    // Rigidbody текущего объекта
    private Rigidbody itemRigidbody;

    // сохраняем параметры useGravity и freezeRotation,
    // чтобы вернуть при отпускании объекта
    private bool itemUseGravity;
    private bool itemFreezeRotation;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // выбираем объект для перемещения
            if (!currentItem)
            {
                RaycastHit hit;
                // посылаем луч из камеры в позицию мыши
                Ray ray = GameCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    // проверяем, попал ли луч в ItemController
                    currentItem = hit.transform.GetComponent<ItemController>();
                    if (currentItem == null)
                        return;
                    else
                    {
                        currentItem.isMoving = true;

                        // инициализируем параметры
                        itemRigidbody = currentItem.rigidbody;
                        itemUseGravity = itemRigidbody.useGravity;
                        itemFreezeRotation = itemRigidbody.freezeRotation;

                        // отключаем гравитацию и вращение
                        itemRigidbody.useGravity = false;
                        itemRigidbody.freezeRotation = true;
                        // приподнимаем объект
                        itemRigidbody.MovePosition(itemRigidbody.position += new Vector3(0, StartHeight, 0));
                    }
                }
            }
            else
            {
                // перемещаем объект за курсором мыши и можем менять высоту колесиком
                Vector3 mousePosition = GameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameCamera.transform.position.y));
                itemRigidbody.MovePosition(new Vector3(mousePosition.x, itemRigidbody.position.y + Input.GetAxis("Mouse ScrollWheel") * HeightStep, mousePosition.z));
            } 
        }
        else if (currentItem)
        {
            // отпускаем объект
            currentItem.isMoving = false;
            itemRigidbody.useGravity = itemUseGravity;
            itemRigidbody.freezeRotation = itemFreezeRotation;
            currentItem = null;
            itemRigidbody = null;
        }
    }
}
