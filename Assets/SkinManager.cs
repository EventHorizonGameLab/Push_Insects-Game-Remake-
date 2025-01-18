using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    [SerializeField] private int defaultSkinIndex = 0;
    public int SelectedSkinIndex { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SelectedSkinIndex = PlayerPrefs.GetInt("SelectedSkinIndex", defaultSkinIndex);
    }

    public void SetSkinIndex(int index)
    {
        SelectedSkinIndex = index;
        PlayerPrefs.SetInt("SelectedSkinIndex", index);
        PlayerPrefs.Save();
    }
}
