using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private List<DifficultyProgress> difficultyProgresses;

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
    }
}
