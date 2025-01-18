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
    [Header("Stars")]//ByEma
    public int starsEarned;//ByEma

    public void SaveRecord(string key, int value)
    {
        if (value == 0) return;
        string uniqueKey = GetUniqueKey(key);
        PlayerPrefs.SetInt(uniqueKey, value);
        PlayerPrefs.Save();
    }

    public int GetRecord(string record)
    {
        string uniqueKey = GetUniqueKey(record);
        return PlayerPrefs.HasKey(uniqueKey) ? PlayerPrefs.GetInt(uniqueKey) : 0;
    }

    string GetUniqueKey(string baseKey)=> $"{baseKey}_Level_{levelID}";
}
public enum Difficulty
{
    BEGINNER,
    INTERMEDIATE,
    ADVANCED
}
