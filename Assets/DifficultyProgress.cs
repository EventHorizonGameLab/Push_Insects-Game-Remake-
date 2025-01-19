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

    private const string PlayerPrefsKey = "DifficultyProgress";

    public void UpdateLevelProgress(int levelID, int stars)
    {
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

            int nextIndex = levels.IndexOf(level) + 1;
            if (nextIndex < levels.Count)
            {
                var nextLevel = levels[nextIndex];
                OnProgressUpdated?.Invoke();
            }
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

    LevelButtonHandler FindHandlerByLevelID(int levelID)
    {
        var handlers = GameObject.FindObjectsOfType<LevelButtonHandler>();
        foreach (var handler in handlers)
        {
            if (handler.levelID == levelID)
            {
                return handler;
            }
        }
        return null;
    }
    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string json = PlayerPrefs.GetString(PlayerPrefsKey);
            JsonUtility.FromJsonOverwrite(json, this);
            OnProgressUpdated?.Invoke();
        }
    }
}