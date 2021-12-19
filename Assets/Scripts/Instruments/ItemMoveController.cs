using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для перемещения объектов
/// </summary>
public class ItemMoveController : MonoBehaviour
{
    [Tooltip("Основная камера")]
    public Camera GameCamera;
    [Tooltip("Высота, на которую приподнимается объект при нажатии")]
    public float StartHeight;

    // текущий объект
    private ItemController currentItem;
    // Rigidbody текущего объекта
    private Rigidbody itemRigidbody;

    // нужно сохранить параметры useGravity и freezeRotation,
    // чтобы вернуть при отпускании объекта
    private bool itemUseGravity;
    private bool itemFreezeRotation;

    // используется для смещения относительно
    // точки нажатия к координатам объекта
    private Vector3 offset;

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

                        // вычисляем смещение
                        offset = itemRigidbody.position - GetMouseWorldPosition();
                        // высоту не учитываем, иначе будет бесконечно подниматься
                        offset.y = 0;
                    }
                }
            }
            else
            {
                // перемещаем объект за курсором мыши и можем менять высоту колесиком
                Vector3 mousePosition = GetMouseWorldPosition();
                // задаем координаты с добавлением смещения
                itemRigidbody.MovePosition(new Vector3(mousePosition.x, itemRigidbody.position.y, mousePosition.z) + offset);
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

    /// <summary>
    /// Получение точки мировых координат из координат точки клика на экране
    /// </summary>
    private Vector3 GetMouseWorldPosition()
    {
        return GameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameCamera.transform.position.y));
    }
}
