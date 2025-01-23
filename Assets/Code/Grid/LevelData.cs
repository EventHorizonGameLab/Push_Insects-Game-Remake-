using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Level ID")]
    public int levelID;
    [Header("Moves")]
    public int movesForMaxScore;
    public int movesForMidScore;
    [Header("Difficulty")]
    public Difficulty difficulty;

    [Header("Progress")]
    [SerializeField] private DifficultyProgress progress;

    public void SaveRecord(string key, int value)
    {
        string uniqueKey = GetUniqueKey(key);
        PlayerPrefs.SetInt(uniqueKey, value);
        PlayerPrefs.Save();
    }

    public int GetRecord(string record)
    {
        string uniqueKey = GetUniqueKey(record);
        return PlayerPrefs.HasKey(uniqueKey) ? PlayerPrefs.GetInt(uniqueKey) : 0;
    }

    string GetUniqueKey(string baseKey) => $"{baseKey}_Level_{levelID}";

    public void CompleteLevel(int movesUsed)
    {
        int stars = CalculateStars(movesUsed);

        if (progress != null)
        {
            progress.UpdateLevelProgress(levelID, stars);
        }
        Debug.Log($"Livello {levelID} completato con {stars} stelle.");
    }

    private int CalculateStars(int movesUsed)
    {
        if (movesUsed <= movesForMaxScore)
            return 3;
        else if (movesUsed <= movesForMidScore)
            return 2;
        else
            return 1;
    }

}
public enum Difficulty
{
    BEGINNER,
    INTERMEDIATE,
    ADVANCED
}