using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [SerializeField] Block_Door doorParent;
    bool interacted;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.instance.PlayerIsDragging() == false && !interacted)
        {
            interacted = true;
            doorParent.OpenDoor();
        }
    }
}
