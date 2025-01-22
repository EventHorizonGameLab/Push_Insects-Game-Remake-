using System.Collections;
using DG.Tweening;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [SerializeField] Block_Door doorParent;
    bool interacted;

    private void OnEnable()
    {
        WaitTween(1).OnComplete(() => interacted = false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.instance.PlayerIsDragging() == false && !interacted)
        {
            doorParent.OpenDoor();
            Debug.Log("porta aperta");
            interacted = true;
        }

    }

 

    Sequence WaitTween(float time)
    {
        return DOTween.Sequence().AppendInterval(time);
    }
}
