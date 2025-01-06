using UnityEngine;

public class LevelData : MonoBehaviour
{
    public void SaveRecord(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public int GetRecord(string record)
    {
        if (PlayerPrefs.HasKey(record)) return PlayerPrefs.GetInt(record);
        else return 0;
    }
}
