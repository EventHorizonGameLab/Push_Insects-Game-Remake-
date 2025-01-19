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