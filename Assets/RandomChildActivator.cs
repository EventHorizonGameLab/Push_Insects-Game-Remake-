using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomChildActivator : MonoBehaviour
{
    private void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        int randomIndex = Random.Range(0, transform.childCount);
        transform.GetChild(randomIndex).gameObject.SetActive(true);
    }
}