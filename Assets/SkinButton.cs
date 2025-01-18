using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private int skinIndex;
    [SerializeField] private GameObject skinPreview;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetSkin);
    }

    private void SetSkin()
    {
        SkinManager.Instance.SetSkinIndex(skinIndex);
        if (skinPreview != null)
        {
            UpdateSkinPreview();
        }
    }
    private void UpdateSkinPreview()
    {
        foreach (Transform child in skinPreview.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (skinIndex >= 0 && skinIndex < skinPreview.transform.childCount)
        {
            skinPreview.transform.GetChild(skinIndex).gameObject.SetActive(true);
        }
    }
}