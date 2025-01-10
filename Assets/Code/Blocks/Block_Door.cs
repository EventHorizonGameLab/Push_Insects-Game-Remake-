using DG.Tweening;
using UnityEngine;

public class Block_Door : MonoBehaviour
{
    Rigidbody rb;
    GameObject keyChild;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        keyChild = transform.GetChild(0).gameObject;
    }
    public void OpenDoor()
    {
        keyChild.SetActive(false);
        gameObject.SetActive(false);
    }
}
