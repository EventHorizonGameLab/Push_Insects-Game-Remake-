using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public int levelID;
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

    string GetUniqueKey(string baseKey)=> $"{baseKey}_Level_{levelID}";
}
