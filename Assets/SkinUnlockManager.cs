using UnityEngine;

public class SkinUnlockManager : MonoBehaviour
{
    [Header("Skin Buttons")]
    [SerializeField] private GameObject[] skinButtons;

    [Header("Difficulty Progress")]
    [SerializeField] private DifficultyProgress beginnerProgress;
    [SerializeField] private DifficultyProgress intermediateProgress;
    [SerializeField] private DifficultyProgress advancedProgress;

    private bool[] unlockedSkins;

    private void Start()
    {
        LoadSkinUnlocks();

        if (beginnerProgress != null)
            beginnerProgress.OnProgressUpdated += CheckUnlocks;

        if (intermediateProgress != null)
            intermediateProgress.OnProgressUpdated += CheckUnlocks;

        if (advancedProgress != null)
            advancedProgress.OnProgressUpdated += CheckUnlocks;

        CheckUnlocks();
    }

    private void OnDestroy()
    {
        if (beginnerProgress != null)
            beginnerProgress.OnProgressUpdated -= CheckUnlocks;

        if (intermediateProgress != null)
            intermediateProgress.OnProgressUpdated -= CheckUnlocks;

        if (advancedProgress != null)
            advancedProgress.OnProgressUpdated -= CheckUnlocks;
    }

    private void LoadSkinUnlocks()
    {
        unlockedSkins = new bool[skinButtons.Length];

        for (int i = 0; i < skinButtons.Length; i++)
        {
            unlockedSkins[i] = PlayerPrefs.GetInt($"SkinUnlocked_{i}", 0) == 1;
        }

        for (int i = 0; i < unlockedSkins.Length; i++)
        {
            if (unlockedSkins[i])
            {
                skinButtons[i]?.SetActive(true);
            }
        }
    }

    private void SaveSkinUnlocks()
    {
        for (int i = 0; i < unlockedSkins.Length; i++)
        {
            PlayerPrefs.SetInt($"SkinUnlocked_{i}", unlockedSkins[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void CheckUnlocks()
    {
        bool updated = false;

        if (beginnerProgress != null && beginnerProgress.AllLevelsCompleted())
        {
            if (!skinButtons[0].activeSelf)
            {
                skinButtons[0]?.SetActive(true);
                updated = true;
            }
        }

        if (beginnerProgress != null && beginnerProgress.AllLevelsPerfect())
        {
            if (!skinButtons[1].activeSelf)
            {
                skinButtons[1]?.SetActive(true);
                updated = true;
            }
        }

        if (intermediateProgress != null && intermediateProgress.AllLevelsCompleted())
        {
            if (!skinButtons[2].activeSelf)
            {
                skinButtons[2]?.SetActive(true);
                updated = true;
            }
        }

        if (intermediateProgress != null && intermediateProgress.AllLevelsPerfect())
        {
            if (!skinButtons[3].activeSelf)
            {
                skinButtons[3]?.SetActive(true);
                updated = true;
            }
        }

        if (advancedProgress != null && advancedProgress.AllLevelsCompleted())
        {
            if (!skinButtons[4].activeSelf)
            {
                skinButtons[4]?.SetActive(true);
                updated = true;
            }
        }

        if (advancedProgress != null && advancedProgress.AllLevelsPerfect())
        {
            if (!skinButtons[5].activeSelf)
            {
                skinButtons[5]?.SetActive(true);
                updated = true;
            }
        }

        if (updated)
        {
            SaveSkinUnlocks();
        }
    }
    private bool UnlockSkin(int index)
    {
        if (index < 0 || index >= unlockedSkins.Length || unlockedSkins[index])
        {
            return false;
        }

        unlockedSkins[index] = true;
        skinButtons[index]?.SetActive(true);
        return true;
    }
}