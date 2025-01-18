using UnityEngine;

public class SkinUnlockManager : MonoBehaviour
{
    [Header("Skin Buttons")]
    [SerializeField] private GameObject[] skinButtons;

    [Header("Level Groups")]
    [SerializeField] private LevelData[] beginnerLevels;
    [SerializeField] private LevelData[] intermediateLevels;
    [SerializeField] private LevelData[] advancedLevels;

    private void Start()
    {
        CheckUnlocks();
    }

    public void CheckUnlocks()
    {
        if (AreLevelsCompleted(beginnerLevels, 1))
            skinButtons[1]?.SetActive(true);

        if (AreLevelsCompleted(beginnerLevels, 3))
            skinButtons[2]?.SetActive(true);

        if (AreLevelsCompleted(intermediateLevels, 1))
            skinButtons[3]?.SetActive(true);

        if (AreLevelsCompleted(intermediateLevels, 3))
            skinButtons[4]?.SetActive(true);

        if (AreLevelsCompleted(advancedLevels, 1))
            skinButtons[5]?.SetActive(true);

        if (AreLevelsCompleted(advancedLevels, 3))
            skinButtons[6]?.SetActive(true);
    }

    private bool AreLevelsCompleted(LevelData[] levels, int requiredStars)
    {
        foreach (LevelData level in levels)
        {
            if (level.starsEarned < requiredStars)
                return false;
        }
        return true;
    }
}
