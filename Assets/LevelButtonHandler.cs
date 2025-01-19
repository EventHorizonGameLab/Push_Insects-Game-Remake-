using UnityEngine;

public class LevelButtonHandler : MonoBehaviour
{
    [Header("Level Information")]
    public int levelID;

    [Header("Button Settings")]
    public bool isFirstLevel;

    [Header("Difficulty Progress")]
    [SerializeField] private DifficultyProgress difficultyProgress;

    [Header("Button Components")]
    [SerializeField] private GameObject interactableButton;
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;

    private void OnEnable()
    {
        if (difficultyProgress != null)
        {
            difficultyProgress.OnProgressUpdated += UpdateButtonState;
        }

        UpdateButtonState();

        CheckIfNextLevel();
    }

    private void OnDisable()
    {
        if (difficultyProgress != null)
        {
            difficultyProgress.OnProgressUpdated -= UpdateButtonState;
        }
    }

    private void CheckIfNextLevel()
    {
        if (difficultyProgress == null) return;

        var levelIndex = difficultyProgress.levels.FindIndex(l => l.levelID == levelID);
        if (levelIndex > 0)
        {
            var previousLevel = difficultyProgress.levels[levelIndex - 1];
            if (previousLevel.isCompleted)
            {
                EnableButton(true);
            }
        }
    }

    private void UpdateButtonState()
    {
        if (difficultyProgress == null)
        {
            Debug.LogWarning($"DifficultyProgress not assigned for button with Level ID: {levelID}");
            return;
        }

        var levelData = difficultyProgress.levels.Find(l => l.levelID == levelID);
        if (levelData != null)
        {
            UpdateButtonVisual(levelData.starsEarned);

            EnableButton(levelData.isCompleted || isFirstLevel);
        }
        else
        {
            Debug.LogWarning($"Level data not found for ID: {levelID}");
        }
    }

    public void UpdateButtonVisual(int starsEarned)
    {
        if (star1 != null) star1.SetActive(starsEarned >= 1);
        if (star2 != null) star2.SetActive(starsEarned >= 2);
        if (star3 != null) star3.SetActive(starsEarned >= 3);
    }

    public void EnableButton(bool enable)
    {
        if (interactableButton != null)
        {
            interactableButton.SetActive(enable);
        }
    }
}