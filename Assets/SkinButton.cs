using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private int skinIndex;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetSkin);
    }

    private void SetSkin()
    {
        SkinManager.Instance.SetSkinIndex(skinIndex);
    }
}