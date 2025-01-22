using System;
using DG.Tweening;
using UnityEngine;

public class Block_Door : MonoBehaviour
{
    public static event Action<bool> OnDoorOpen;

    public enum DoorAnimation
    {
        Black,
        Blue,
        Green
    }
    private Animator animator;
    Rigidbody rb;
    GameObject keyChild;
    [SerializeField] Collider doorCollider;
    Vector3 initialPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        keyChild = transform.GetChild(0).gameObject;
        initialPosition = transform.position;
    }
    private void OnEnable()
    {
        State_Manager.OnReset += ResetDoor;
        GameManager.OnLevelCompleted += ResetLogic;
    }
    private void OnDisable()
    {
        State_Manager.OnReset -= ResetDoor;
        GameManager.OnLevelCompleted -= ResetLogic;
    }
    public void OpenDoor()
    {
        OnDoorOpen?.Invoke(true);
        keyChild.SetActive(false);
        DoorAnimation randomAnimation = (DoorAnimation)UnityEngine.Random.Range(0, 3);
        switch (randomAnimation)
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

    void ResetDoor()
    {
        OnDoorOpen?.Invoke(false);
        doorCollider.enabled = true;
        keyChild.SetActive(true);
        animator.Rebind();
        animator.Update(0f);
        transform.position = initialPosition;
    }

    void ResetLogic()
    {
        OnDoorOpen?.Invoke(true);
    }
}
