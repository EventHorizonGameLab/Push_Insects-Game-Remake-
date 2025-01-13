using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Block_Sliding_X : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IBlock
{
    [SerializeField] private float slideSpeed = 30f;
    [SerializeField] private float dragThreshold = 0.25f;

    private Rigidbody rb;
    private Collider blockCollider;
    private bool isSliding = false;
    private float dragDirection;

    private float maxX;
    private float minX;

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

        float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);

        Vector3 newPosition = new Vector3(clampedX, transform.position.y, transform.position.z);
        rb.MovePosition(newPosition);
        dragDirection = targetPosition.x > positionBeforeSlide.x ? 1 : -1;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSliding) return;

        float dragDistance = Mathf.Abs(transform.position.x - positionBeforeSlide.x);

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
        float targetX = direction > 0 ? maxX : minX;
        maxX = RoundToNearestHalf(maxX);
        minX = RoundToNearestHalf(minX);
        StartCoroutine(SlideToTarget(new Vector3(targetX, transform.position.y, transform.position.z)));
    }

    private float RoundToNearestHalf(float value)
    {
        return Mathf.Round(value * 2f) / 2f;
    }

    private IEnumerator SlideToTarget(Vector3 targetPosition)
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

    private void ResetToInitialPosition()
    {
        rb.MovePosition(positionBeforeSlide);
        positionAfterSlide = positionBeforeSlide; // Aggiorna la posizione finale per la verifica
        GiveMoveInfo();
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
