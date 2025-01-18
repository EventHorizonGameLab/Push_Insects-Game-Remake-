using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToDisable;
    [SerializeField] GameObject[] objectsToEnable;

    public void EnableObjects()
    {
        foreach (GameObject obj in objectsToEnable) obj.SetActive(true);
    }

    public void DisableObjects()
    {
        foreach (GameObject obj in objectsToDisable) obj.SetActive(false);
    }
}
