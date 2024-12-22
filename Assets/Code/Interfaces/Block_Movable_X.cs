using System;
using UnityEngine;
using UnityEngine.EventSystems; // Necessario per le interfacce drag

[RequireComponent(typeof(Rigidbody))]
public class Block_Movable_X : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IBlock
{
    private Rigidbody rb;
    private Collider blockCollider;
    private float zDepth;
    private bool isDragging = false;
    private Vector3 lastValidPosition;

    private float maxX; // Limite massimo verso destra
    private float minX; // Limite massimo verso sinistra

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

        // Ottieni le dimensioni del blocco
        Vector3 halfExtents = blockCollider.bounds.extents;
        float blockWidth = halfExtents.x * 2; // Larghezza totale del blocco (unità della griglia)

        // Calcola la posizione allineata
        Vector3 alignedPosition;

        if (Mathf.RoundToInt(blockWidth) % 2 == 0) // Dimensione pari
        {
            // Calcola il bordo sinistro del blocco
            float leftEdge = lastValidPosition.x - halfExtents.x;

            // Arrotonda il bordo sinistro alla griglia
            float alignedLeftEdge = Mathf.Round(leftEdge);

            // Ricalcola il centro basandosi sul bordo sinistro allineato
            float alignedCenterX = alignedLeftEdge + halfExtents.x;

            // Imposta la nuova posizione allineata
            alignedPosition = new Vector3(alignedCenterX, lastValidPosition.y, lastValidPosition.z);
        }
        else // Dimensione dispari
        {
            // Arrotonda direttamente il centro alla griglia
            float alignedCenterX = Mathf.Round(lastValidPosition.x);

            // Imposta la nuova posizione allineata
            alignedPosition = new Vector3(alignedCenterX, lastValidPosition.y, lastValidPosition.z);
        }

        // Muovi il blocco alla posizione allineata
        rb.MovePosition(alignedPosition);

    }




    private void UpdateLimitsWithRaycast()
    {
        Vector3 halfExtents = blockCollider.bounds.extents;
        
        // Origin of raycasts
        Vector3 rightOrigin = transform.position + Vector3.right * halfExtents.x;
        Vector3 leftOrigin = transform.position + Vector3.left * halfExtents.x;
        
        // Raycast to the right
        if (Physics.Raycast(rightOrigin, Vector3.right, out RaycastHit hitRight, Mathf.Infinity))
        {
            maxX = hitRight.point.x - halfExtents.x; // Subtract half width to avoid overlap
        }
        else
        {
            maxX = Mathf.Infinity; // No obstacle
        }

        // Raycast to the left
        if (Physics.Raycast(leftOrigin, Vector3.left, out RaycastHit hitLeft, Mathf.Infinity))
        {
            minX = hitLeft.point.x + halfExtents.x; // Add half width to avoid overlap
        }
        else
        {
            minX = -Mathf.Infinity; // No obstacle
        }

        // Debug raycasts
        Debug.DrawRay(rightOrigin, Vector3.right * (maxX - rightOrigin.x), Color.green);
        Debug.DrawRay(leftOrigin, Vector3.left * (leftOrigin.x - minX), Color.red);
    }

    
}