using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Compass, Torch, Waterbottle };

[RequireComponent(typeof(Rigidbody))]
public class ItemController : MonoBehaviour
{
    public float Weight;
    public string Name;
    public string Id;
    public ItemType Type;

    public float LerpSpeed;

    public new Rigidbody rigidbody { get; private set; }

    [HideInInspector]
    // перемещаем ли мы объект в данный момент
    public bool isMoving = false;
    private new BoxCollider collider;
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

    public void PutInBackpack()
    {
        collider.enabled = false;
        rigidbody.isKinematic = true;
        StartCoroutine(MoveWithLerp(transform.localPosition, Vector3.zero, transform.localRotation, Quaternion.identity));
    }

    public void PullOutBackpack()
    {
        transform.SetParent(parent);
        StartCoroutine(PullOutBackpackRoutine(transform.localPosition, startedPos, transform.localRotation, startedRot));
    }

    private IEnumerator MoveWithLerp(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot)
    {
        float startTime = Time.time;
        float interpolant = 0;
        while (interpolant < 1)
        {
            interpolant = (Time.time - startTime) * LerpSpeed;

            Vector3 curPos = Vector3.Lerp(startPos, endPos, interpolant);
            transform.localPosition = curPos;

            Quaternion curRot = Quaternion.Lerp(startRot, endRot, interpolant);
            transform.localRotation = curRot;

            yield return null;
        }
    }

    private IEnumerator PullOutBackpackRoutine(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot)
    {
        yield return MoveWithLerp(startPos, endPos, startRot, endRot);
        collider.enabled = true;
        rigidbody.isKinematic = false;
    }
}
