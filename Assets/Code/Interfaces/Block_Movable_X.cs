using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Block_Movable_X : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IBlock
{
    private Rigidbody rb;
    private Collider blockCollider;
    private float zDepth;
    private bool isDragging = false;
    private Vector3 lastValidPosition;

    private float maxX;
    private float minX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        blockCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        UpdateLimitsWithRaycast();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
        isDragging = true;
        lastValidPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zDepth));
        Vector3 targetPosition = new Vector3(worldPosition.x, transform.position.y, transform.position.z);

        if (targetPosition.x >= minX && targetPosition.x <= maxX)
        {
            rb.MovePosition(targetPosition);
            lastValidPosition = targetPosition;
        }
        else
        {
            rb.MovePosition(lastValidPosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        Vector3 halfExtents = blockCollider.bounds.extents;
        float blockWidth = halfExtents.x * 2;

        Vector3 alignedPosition;

        if (Mathf.RoundToInt(blockWidth) % 2 == 0)
        {
            float alignedX = RoundToNearestHalf(lastValidPosition.x);
            alignedPosition = new Vector3(alignedX, lastValidPosition.y, lastValidPosition.z);
        }
        else
        {
            float alignedCenterX = Mathf.Round(lastValidPosition.x);
            alignedPosition = new Vector3(alignedCenterX, lastValidPosition.y, lastValidPosition.z);
        }

        rb.MovePosition(alignedPosition);
    }

    private void UpdateLimitsWithRaycast()
    {
        Vector3 halfExtents = blockCollider.bounds.extents;
        Vector3 rightOrigin = transform.position + Vector3.right * halfExtents.x;
        Vector3 leftOrigin = transform.position + Vector3.left * halfExtents.x;

        if (Physics.Raycast(rightOrigin, Vector3.right, out RaycastHit hitRight, Mathf.Infinity))
        {
            maxX = hitRight.point.x - halfExtents.x;
        }
        else
        {
            maxX = Mathf.Infinity;
        }

        if (Physics.Raycast(leftOrigin, Vector3.left, out RaycastHit hitLeft, Mathf.Infinity))
        {
            minX = hitLeft.point.x + halfExtents.x;
        }
        else
        {
            minX = -Mathf.Infinity;
        }

        Debug.DrawRay(rightOrigin, Vector3.right * (maxX - rightOrigin.x), Color.green);
        Debug.DrawRay(leftOrigin, Vector3.left * (leftOrigin.x - minX), Color.red);
    }

    private float RoundToNearestHalf(float value)
    {
        float roundedValue = Mathf.Round(value * 2f) / 2f; // es: 1.47 *2 = 2.94, roundato 3, diviso 2 = 1.5 [ottengo multiplo di o.5]

        if (Mathf.Abs(roundedValue % 1) < Mathf.Epsilon)
        {
            return value > roundedValue ? roundedValue + 0.5f : roundedValue - 0.5f;
        }

        return roundedValue;
    }
}