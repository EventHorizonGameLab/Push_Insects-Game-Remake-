using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [SerializeField] Block_Door parent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPlayer>(out _))
        {
            // lgoica per chiamare evento o metodo nel parent per aprire la porta
        }
    }
}
