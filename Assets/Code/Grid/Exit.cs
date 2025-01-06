using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //TODO: invoke endlevel event
        }
    }
}
