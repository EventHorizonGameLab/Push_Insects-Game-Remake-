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
}
