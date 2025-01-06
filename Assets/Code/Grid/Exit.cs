using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("un oggetto e entrato");
        if (other.gameObject.CompareTag("Player"))
        {
            //TODO: invoke endlevel event
            Debug.Log("USCITA RILEVATA");
        }
    }
}
