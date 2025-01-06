using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Block_Movable_Z : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private Rigidbody rb;
    private Collider blockCollider;
    private float zDepth;
    private bool isDragging = false;
    private Vector3 lastValidPosition;
    private Vector3 positionBeforeDrag;
    private Vector3 positionsAfterDrag;
    private Vector3 dragOffset;

    private float maxZ, minZ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        blockCollider = GetComponent<Collider>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateLimitsWithRaycast();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.OnPlayerDragging(true);
        positionBeforeDrag = transform.position;
        zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
        isDragging = true;
        lastValidPosition = transform.position;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zDepth));
        dragOffset = transform.position - worldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zDepth));
        Vector3 targetPosition = worldPosition + dragOffset;
        targetPosition = new Vector3(transform.position.x, transform.position.y, targetPosition.z);

        if (targetPosition.z >= minZ && targetPosition.z <= maxZ)
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
        float blockDepth = halfExtents.z * 2;

        Vector3 alignedPosition;

        if (Mathf.RoundToInt(blockDepth) % 2 == 0)
        {
            float alignedZ = RoundToNearestHalf(lastValidPosition.z);
            alignedPosition = new Vector3(lastValidPosition.x, lastValidPosition.y, alignedZ);
        }
        else
        {
            float alignedCenterZ = Mathf.Round(lastValidPosition.z);
            alignedPosition = new Vector3(lastValidPosition.x, lastValidPosition.y, alignedCenterZ);
        }

        rb.MovePosition(alignedPosition);
        positionsAfterDrag = alignedPosition;
        GiveMoveInfo();
        GameManager.OnPlayerDragging(false);
    }

    private void UpdateLimitsWithRaycast()
    {
        Vector3 halfExtents = blockCollider.bounds.extents;
        Vector3 centerOrigin = transform.position;

        if (Physics.Raycast(centerOrigin, Vector3.forward, out RaycastHit hitFront, Mathf.Infinity) && hitFront.collider != blockCollider)
            maxZ = hitFront.point.z - halfExtents.z;
        else
            maxZ = Mathf.Infinity;

        if (Physics.Raycast(centerOrigin, Vector3.back, out RaycastHit hitBack, Mathf.Infinity) && hitBack.collider != blockCollider)
            minZ = hitBack.point.z + halfExtents.z;
        else
            minZ = -Mathf.Infinity;

        Debug.DrawRay(centerOrigin, Vector3.forward * (maxZ - centerOrigin.z), Color.blue);
        Debug.DrawRay(centerOrigin, Vector3.back * (centerOrigin.z - minZ), Color.yellow);
    }

    private float RoundToNearestHalf(float value)
    {
        float roundedValue = Mathf.Round(value * 2f) / 2f;

        if (Mathf.Abs(roundedValue % 1) < Mathf.Epsilon)
        {
            return value > roundedValue ? roundedValue + 0.5f : roundedValue - 0.5f;
        }

        return roundedValue;
    }

    void GiveMoveInfo()
    {
        if (positionsAfterDrag != positionBeforeDrag) { GameManager.OnMoveMade?.Invoke(); }
        else { Debug.Log("Mossa non usata"); }
    }
}
