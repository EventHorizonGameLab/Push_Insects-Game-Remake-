using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Block_Movable_XZ : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IBlock, IPointerDownHandler, IPointerUpHandler
{
    private Rigidbody rb;
    private Collider blockCollider;
    private float zDepth;
    private bool isDragging = false;
    private bool exitAvailable;
    private bool isSliding;
    private Vector3 lastValidPosition;
    private Vector3 positionBeforeDrag;
    private Vector3 positionsAfterDrag;
    private Vector3 dragOffset;

    private float maxX, minX;
    private float maxZ, minZ;

    private bool isHorizontalDrag = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        blockCollider = GetComponent<Collider>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        gameObject.layer = 6;
        //ExcludeCollisions();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CheckForExit();
        SlideToExitIfAvailable();
        UpdateLimitsWithRaycast();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSliding) return;
        GameManager.OnPlayerDragging(true);
        positionBeforeDrag = transform.position;
        zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
        isDragging = true;
        lastValidPosition = transform.position;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zDepth));
        dragOffset = transform.position - worldPosition;
        // Determina se il trascinamento inizia in orizzontale o verticale
        isHorizontalDrag = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || isSliding) return;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zDepth));
        Vector3 targetPosition = worldPosition + dragOffset;

        if (isHorizontalDrag) // Movimento orizzontale (asse X)
        {
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
        else // Movimento verticale (asse Z)
        {
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSliding) return;
        isDragging = false;

        Vector3 halfExtents = blockCollider.bounds.extents;
        float blockWidth = halfExtents.x * 2;
        float blockDepth = halfExtents.z * 2;

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

        if (Mathf.RoundToInt(blockDepth) % 2 == 0)
        {
            float alignedZ = RoundToNearestHalf(lastValidPosition.z);
            alignedPosition.z = alignedZ;
        }
        else
        {
            float alignedCenterZ = Mathf.Round(lastValidPosition.z);
            alignedPosition.z = alignedCenterZ;
        }

        rb.MovePosition(alignedPosition);

        positionsAfterDrag = alignedPosition;

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

        if (Physics.Raycast(centerOrigin, Vector3.forward, out RaycastHit hitFront, Mathf.Infinity) && hitFront.collider != blockCollider)
            maxZ = hitFront.point.z - halfExtents.z;
        else
            maxZ = Mathf.Infinity;

        if (Physics.Raycast(centerOrigin, Vector3.back, out RaycastHit hitBack, Mathf.Infinity) && hitBack.collider != blockCollider)
            minZ = hitBack.point.z + halfExtents.z;
        else
            minZ = -Mathf.Infinity;

        Debug.DrawRay(centerOrigin, Vector3.right * (maxX - centerOrigin.x), Color.green);
        Debug.DrawRay(centerOrigin, Vector3.left * (centerOrigin.x - minX), Color.red);
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

    void CheckForExit()
    {
        if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit hitCheck, 1000))
        {
            if (hitCheck.transform.gameObject.layer == 7 && !isDragging) exitAvailable = true;
            else exitAvailable = false;
        }
    }

    void SlideToExitIfAvailable()
    {
        if (exitAvailable)
        {
            isSliding = true;
            rb.isKinematic = false;
            rb.AddForce(Vector3.right * 25, ForceMode.VelocityChange);
            Physics.SyncTransforms();
        }

    }

    void GiveMoveInfo()
    {
        if (positionsAfterDrag != positionBeforeDrag) { GameManager.OnMoveMade?.Invoke(); }
        else { Debug.Log("Mossa non usata"); }
    }

    void ExcludeCollisions()
    {
        int layerA = gameObject.layer; // Layer dell'oggetto corrente

        for (int i = 0; i < 32; i++) // Unity supporta 32 layer
        {
            if (i != 7 && i != 8) // Ignora layer 7 e 8
            {
                Physics.IgnoreLayerCollision(layerA, i, true); // Disabilita le collisioni
            }
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.OnPlayerDragging?.Invoke(false);
    }
}

