using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Место для объектов определенного типа на рюкзаке
/// </summary>
public class ItemBackpackPlace : MonoBehaviour
{
    [Tooltip("Тип объекта, для которого предназначено место")]
    public ItemType Type;
}
