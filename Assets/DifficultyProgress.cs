using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyProgress", menuName = "Progression/DifficultyProgress")]
public class DifficultyProgress : ScriptableObject
{
    [System.Serializable]
    public class LevelProgress
    {
        public int levelID;
        public int starsEarned;
        public bool isCompleted;
    }

    public List<LevelProgress> levels = new List<LevelProgress>();
    public event Action OnProgressUpdated;

    private string PlayerPrefsKey => $"DifficultyProgress_{name}";

    public void UpdateLevelProgress(int levelID, int stars)
    {
        Debug.Log($"Updating progress for level {levelID} in {name} with {stars} stars.");

        var level = levels.Find(l => l.levelID == levelID);
        if (level != null)
        {
            if (stars > level.starsEarned)
            {
                level.starsEarned = stars;
            }

            level.isCompleted = true;
            OnProgressUpdated?.Invoke();
            SaveProgress();
        }
        else
        {
            Debug.LogError($"Level with ID {levelID} not found in {name}");
        }
    }

    public bool AllLevelsCompleted()
    {
        return levels.TrueForAll(l => l.isCompleted);
    }

    public bool AllLevelsPerfect()
    {
        return levels.TrueForAll(l => l.starsEarned == 3);
    }

    public void SaveProgress()
    {
        Debug.Log($"Saving progress for {name}.");
        var savedData = new SavedProgress
        {
            levelProgresses = new List<SavedLevelProgress>()
        };

        foreach (var level in levels)
        {
            savedData.levelProgresses.Add(new SavedLevelProgress
            {
                levelID = level.levelID,
                starsEarned = level.starsEarned,
                isCompleted = level.isCompleted
            });
        }

        string json = JsonUtility.ToJson(savedData);
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
        Debug.Log($"Progress saved for {name}: {json}");
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string json = PlayerPrefs.GetString(PlayerPrefsKey);
            Debug.Log($"Loading progress for {name}: {json}");
            var savedData = JsonUtility.FromJson<SavedProgress>(json);

            foreach (var savedLevel in savedData.levelProgresses)
            {
                var level = levels.Find(l => l.levelID == savedLevel.levelID);
                if (level != null)
                {
                    level.starsEarned = savedLevel.starsEarned;
                    level.isCompleted = savedLevel.isCompleted;
                }
            }

            OnProgressUpdated?.Invoke();
        }
        else
        {
            Debug.LogWarning($"No progress found for {name} in PlayerPrefs.");
        }
    }

    [Serializable]
    private class SavedProgress
    {
        public List<SavedLevelProgress> levelProgresses;
    }

    [Serializable]
    private class SavedLevelProgress
    {
        public int levelID;
        public int starsEarned;
        public bool isCompleted;
    }
}