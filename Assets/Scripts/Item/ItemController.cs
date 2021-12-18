using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// типы объектов
public enum ItemType { Compass, Torch, Waterbottle };

/// <summary>
/// Контроллер объекта
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ItemController : MonoBehaviour
{
    [Tooltip("Вес объекта")]
    public float Weight;
    [Tooltip("Имя объекта")]
    public string Name;
    [Tooltip("Идентификатор объекта")]
    public string Id;
    [Tooltip("Тип объекта")]
    public ItemType Type;

    [Tooltip("Скорость интерполяции")]
    public float LerpSpeed;

    public new Rigidbody rigidbody { get; private set; }

    [HideInInspector]
    // перемещаем ли мы объект в данный момент
    public bool isMoving = false;
    private new BoxCollider collider;
    // сохраняем начальные родителя, позицию и поворот, чтобы вернуть
    // в то же место при доставании из рюкзака
    private Transform parent;
    private Vector3 startedPos;
    private Quaternion startedRot;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        rigidbody.mass = Weight;
        parent = transform.parent;
        startedPos = transform.localPosition;
        startedRot = transform.localRotation;
    }

    /// <summary>
    /// Добавление в рюкзак - отключаем перемещение и переносим в
    /// соответствующее место на рюкзаке
    /// </summary>
    public void PutInBackpack()
    {
        collider.enabled = false;
        rigidbody.isKinematic = true;
        StartCoroutine(MoveWithLerp(transform.localPosition, Vector3.zero, transform.localRotation, Quaternion.identity));
    }

    /// <summary>
    /// Доставание из рюкзака - включаем перемещение и переносим в
    /// начальное состояние
    /// </summary>
    public void PullOutBackpack()
    {
        transform.SetParent(parent);
        StartCoroutine(PullOutBackpackRoutine(transform.localPosition, startedPos, transform.localRotation, startedRot));
    }

    /// <summary>
    /// Перемещение и поворот с интерполяцией
    /// </summary>
    private IEnumerator MoveWithLerp(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot)
    {
        float startTime = Time.time;
        float interpolant = 0;
        while (interpolant < 1)
        {
            // высчитываем параметр для определения текущего значения позиции и поворота
            // в зависимости от времени от начала интерполяции 
            interpolant = (Time.time - startTime) * LerpSpeed;

            // текущая позиция
            Vector3 curPos = Vector3.Lerp(startPos, endPos, interpolant);
            transform.localPosition = curPos;

            // текущий поворот
            Quaternion curRot = Quaternion.Lerp(startRot, endRot, interpolant);
            transform.localRotation = curRot;

            yield return null;
        }
    }

    /// <summary>
    /// В случае доставания из рюкзака - сначала возвращаем в
    /// начальное состояние, потом включаем перемещение
    /// </summary>
    private IEnumerator PullOutBackpackRoutine(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot)
    {
        yield return MoveWithLerp(startPos, endPos, startRot, endRot);
        collider.enabled = true;
        rigidbody.isKinematic = false;
    }
}
