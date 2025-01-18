using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Block_Sliding_Z : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IBlock
{
    [SerializeField] private float slideSpeed = 30f;
    [SerializeField] private float dragThreshold = 0.25f;

    private Rigidbody rb;
    private Collider blockCollider;
    private bool isSliding = false;
    private float dragDirection;

    private float maxZ;
    private float minZ;

    private Vector3 positionBeforeSlide;
    private Vector3 positionAfterSlide;
    private Vector3 dragOffset;

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
        if (isSliding) return;

        positionBeforeSlide = transform.position;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.WorldToScreenPoint(transform.position).z));
        dragOffset = transform.position - worldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSliding) return;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.WorldToScreenPoint(transform.position).z));
        Vector3 targetPosition = worldPosition + dragOffset;

        float clampedZ = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
        dragDirection = targetPosition.z > positionBeforeSlide.z ? 1 : -1;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSliding) return;

        float dragDistance = Mathf.Abs(transform.position.z - positionBeforeSlide.z);

        if (dragDistance >= dragThreshold)
        {
            Slide(dragDirection);
        }
        else
        {
            ResetToInitialPosition();
        }
    }

    private void Slide(float direction)
    {
        isSliding = true;
        maxZ = RoundToNearestHalf(maxZ);
        minZ = RoundToNearestHalf(minZ);
        float targetZ = direction > 0 ? maxZ : minZ;
        StartCoroutine(SlideToTarget(new Vector3(transform.position.x, transform.position.y, targetZ)));
    }

    private float RoundToNearestHalf(float value)
    {
        return Mathf.Round(value * 2f) / 2f;
    }

    private System.Collections.IEnumerator SlideToTarget(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        transform.position = targetPosition;
        positionAfterSlide = transform.position;
        GiveMoveInfo();
        isSliding = false;
    }

    private void ResetToInitialPosition()
    {
        transform.position = positionBeforeSlide;
        positionAfterSlide = positionBeforeSlide; // Aggiorna la posizione finale per la verifica
        GiveMoveInfo();
    }

    private void GiveMoveInfo()
    {
        if (Vector3.Distance(positionAfterSlide, positionBeforeSlide) > 0.01f)
        {
            GameManager.OnMoveMade?.Invoke();
            GameManager.OnMoveToRegister?.Invoke(this, positionBeforeSlide);
        }
        else
        {
            Debug.Log("Mossa non usata");
        }
    }

    private void UpdateLimitsWithRaycast()
    {
        Vector3 halfExtents = blockCollider.bounds.extents;
        Vector3 centerOrigin = transform.position;

        if (Physics.Raycast(centerOrigin, Vector3.forward, out RaycastHit hitForward, Mathf.Infinity) && hitForward.collider != blockCollider)
            maxZ = hitForward.point.z - halfExtents.z;
        else
            maxZ = Mathf.Infinity;

        if (Physics.Raycast(centerOrigin, Vector3.back, out RaycastHit hitBack, Mathf.Infinity) && hitBack.collider != blockCollider)
            minZ = hitBack.point.z + halfExtents.z;
        else
            minZ = -Mathf.Infinity;

        Debug.DrawRay(centerOrigin, Vector3.forward * (maxZ - centerOrigin.z), Color.green);
        Debug.DrawRay(centerOrigin, Vector3.back * (centerOrigin.z - minZ), Color.red);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.OnPlayerDragging?.Invoke(false);
    }

    public void RestorePositionTo(Vector3 position)
    {
        rb.MovePosition(position);
        GameManager.OnMoveUndone?.Invoke();
    }
}
