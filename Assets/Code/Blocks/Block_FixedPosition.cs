using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Block_FixedPosition : MonoBehaviour
{
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
