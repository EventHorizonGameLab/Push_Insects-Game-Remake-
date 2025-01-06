using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] int levelID;
    public void SaveRecord(string key, int value)
    {
        string uniqueKey = GetUniqueKey(key);
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public int GetRecord(string record)
    {
        if (PlayerPrefs.HasKey(record)) return PlayerPrefs.GetInt(record);
        else return 0;
    }

    string GetUniqueKey(string baseKey)=> $"{baseKey}_Level_{levelID}";
}
