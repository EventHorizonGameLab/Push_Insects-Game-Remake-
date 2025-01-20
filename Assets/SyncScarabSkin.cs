using UnityEngine;

public class SyncScarabSkin : MonoBehaviour
{
    [SerializeField] private Transform originalObject;
    [SerializeField] private Transform copyObject;
    private void OnEnable()
    {
        SyncActiveChild();
    }

    private void SyncActiveChild()
    {
        if (originalObject == null || copyObject == null)
        {
            Debug.LogError("Original or Copy object not assigned.");
            return;
        }
        int activeChildIndex = -1;
        for (int i = 0; i < originalObject.childCount; i++)
        {
            if (originalObject.GetChild(i).gameObject.activeSelf)
            {
                activeChildIndex = i;
                break;
            }
        }
        if (activeChildIndex != -1)
        {
            for (int i = 0; i < copyObject.childCount; i++)
            {
                copyObject.GetChild(i).gameObject.SetActive(i == activeChildIndex);
            }
        }
    }
}