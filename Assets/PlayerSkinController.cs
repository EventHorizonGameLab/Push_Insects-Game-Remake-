using UnityEngine;

public class PlayerSkinController : MonoBehaviour
{
    private void Start()
    {
        int selectedSkinIndex = SkinManager.Instance.SelectedSkinIndex;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        if (selectedSkinIndex >= 0 && selectedSkinIndex < transform.childCount)
        {
            transform.GetChild(selectedSkinIndex).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
