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
    private Vector3 positionBeforeDrag;
    private Vector3 positionsAfterDrag;
    private Vector3 dragOffset;

    private float maxX, minX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        blockCollider = GetComponent<Collider>();
    }

    private void Update()
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
        targetPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);

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
        positionsAfterDrag = alignedPosition;
        GiveMoveInfo();
        GameManager.OnPlayerDragging(false);
    }

    private void UpdateLimitsWithRaycast()
    {
        Vector3 halfExtents = blockCollider.bounds.extents;
        Vector3 centerOrigin = transform.position;

        if (Physics.Raycast(centerOrigin, Vector3.right, out RaycastHit hitRight, Mathf.Infinity) && hitRight.collider != blockCollider)
            maxX = hitRight.point.x - halfExtents.x;
        else
            maxX = Mathf.Infinity;

        if (Physics.Raycast(centerOrigin, Vector3.left, out RaycastHit hitLeft, Mathf.Infinity) && hitLeft.collider != blockCollider)
            minX = hitLeft.point.x + halfExtents.x;
        else
            minX = -Mathf.Infinity;

        Debug.DrawRay(centerOrigin, Vector3.right * (maxX - centerOrigin.x), Color.green);
        Debug.DrawRay(centerOrigin, Vector3.left * (centerOrigin.x - minX), Color.red);
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
        if (positionsAfterDrag != positionBeforeDrag) { Debug.Log("Mossa usata"); }
        else { Debug.Log("Mossa non usata"); }
    }
}
