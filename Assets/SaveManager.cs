using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private List<DifficultyProgress> difficultyProgresses;
    [SerializeField] private GameObject[] skinButtons;

    private void Awake()
    {
        Debug.Log("SaveManager Awake: Initiating load of all progress.");
        LoadAllProgress();
    }

    private void Start()
    {
        Debug.Log("SaveManager Start: Ensuring all progress is loaded.");
        LoadAllProgress();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("SaveManager OnApplicationQuit: Saving all progress.");
        SaveAllProgress();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("SaveManager OnApplicationPause: Saving all progress.");
            SaveAllProgress();
        }
    }

    public void SaveAllProgress()
    {
        Debug.Log("SaveManager: Saving all progress...");
        foreach (var progress in difficultyProgresses)
        {
            if (progress != null)
            {
                Debug.Log($"Saving progress for {progress.name}");
                progress.SaveProgress();
            }
        }
    }

    public void LoadAllProgress()
    {
        Debug.Log("SaveManager: Loading all progress...");
        foreach (var progress in difficultyProgresses)
        {
            if (progress != null)
            {
                Debug.Log($"Loading progress for {progress.name}");
                progress.LoadProgress();
            }
        }
        InvokeCheckIfNextLevelOnButtons();
    }

    public void ResetAllProgress()
    {
        Debug.Log("SaveManager: Resetting all progress...");

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared.");

        foreach (var progress in difficultyProgresses)
        {
            if (progress != null)
            {
                foreach (var level in progress.levels)
                {
                    level.starsEarned = 0;
                    level.isCompleted = false;
                }

                progress.SaveProgress();

                Debug.Log($"Progress reset for {progress.name}");
            }
        }

        if (skinButtons != null && skinButtons.Length > 0)
        {
            for (int i = 0; i < skinButtons.Length; i++)
            {
                if (skinButtons[i] != null)
                {
                    skinButtons[i].SetActive(i == 0);
                }
            }

            var skinButton = skinButtons[0].GetComponent<SkinButton>();
            if (skinButton != null)
            {
                Debug.Log("Calling SetSkin on the first SkinButton.");
                skinButton.SendMessage("SetSkin");
            }
        }

        LoadAllProgress();
        Debug.Log("Progress reset complete and loaded into the game.");
    }
    private void InvokeCheckIfNextLevelOnButtons()
    {
        var levelButtons = Object.FindObjectsByType<LevelButtonHandler>(FindObjectsSortMode.None);

        foreach (var button in levelButtons)
        {
            button.CheckIfNextLevel();
        }

        Debug.Log("Called CheckIfNextLevel on all LevelButtonHandler instances.");
    }
}
