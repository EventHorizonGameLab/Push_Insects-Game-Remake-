using UnityEngine;

public class SkinUnlockManager : MonoBehaviour
{
    [Header("Skin Buttons")]
    [SerializeField] private GameObject[] skinButtons;

    [Header("Difficulty Progress")]
    [SerializeField] private DifficultyProgress beginnerProgress;
    [SerializeField] private DifficultyProgress intermediateProgress;
    [SerializeField] private DifficultyProgress advancedProgress;

    private void Start()
    {
        CheckUnlocks();
    }

    public void CheckUnlocks()
    {
        if (beginnerProgress != null && beginnerProgress.AllLevelsCompleted())
            skinButtons[1]?.SetActive(true);

        if (beginnerProgress != null && beginnerProgress.AllLevelsPerfect())
            skinButtons[2]?.SetActive(true);

        if (intermediateProgress != null && intermediateProgress.AllLevelsCompleted())
            skinButtons[3]?.SetActive(true);

        if (intermediateProgress != null && intermediateProgress.AllLevelsPerfect())
            skinButtons[4]?.SetActive(true);

        if (advancedProgress != null && advancedProgress.AllLevelsCompleted())
            skinButtons[5]?.SetActive(true);

        if (advancedProgress != null && advancedProgress.AllLevelsPerfect())
            skinButtons[6]?.SetActive(true);
    }
}