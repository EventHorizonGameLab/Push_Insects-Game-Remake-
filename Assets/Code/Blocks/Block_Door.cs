using DG.Tweening;
using UnityEngine;

public class Block_Door : MonoBehaviour
{
    public enum DoorAnimation
    {
        Black,
        Blue,
        Green
    }
    [Header("Door Animation Settings")]
    public DoorAnimation selectedAnimation; // Campo visibile dall'Inspector
    private Animator animator;
    Rigidbody rb;
    GameObject keyChild;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        keyChild = transform.GetChild(0).gameObject;
    }
    public void OpenDoor()
    {
        keyChild.SetActive(false);
        switch (selectedAnimation)
        {
            case DoorAnimation.Black:
                animator.SetTrigger("Black");
                break;
            case DoorAnimation.Blue:
                animator.SetTrigger("Blue");
                break;
            case DoorAnimation.Green:
                animator.SetTrigger("Green");
                break;
        }
    }
}
