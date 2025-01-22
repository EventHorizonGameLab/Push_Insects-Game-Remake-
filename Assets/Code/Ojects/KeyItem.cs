using System.Collections;
using DG.Tweening;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [SerializeField] Block_Door doorParent;
    bool interacted;
    bool checkAvailable;

    private void OnEnable()
    {
        WaitTween(1).OnComplete(() => interacted = false);
        GameManager.OnMoveMade += CheckTime;
    }

    private void OnDisable()
    {
        GameManager.OnMoveMade -= CheckTime;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.instance.PlayerIsDragging() == false && !interacted && checkAvailable)
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

    void CheckTime()
    {
        WaitTween(0.5f).OnComplete(() =>
        {
            checkAvailable = true;
            WaitTween(0.5f).OnComplete(() => checkAvailable = false);
        });
    }

}
