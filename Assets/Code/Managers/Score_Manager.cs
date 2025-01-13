using System.Collections.Generic;
using UnityEngine;

public class Score_Manager : MonoBehaviour
{
    [SerializeField] List<GameObject> seeds;
    Score CalculateScore(LevelData levelData, int movesMade) // min 1, max 3
    {
        if (movesMade <= levelData.movesForMaxScore) return Score.MAX;
        if (movesMade <= levelData.movesForMidScore) return Score.MID;
        return Score.MIN;
    }
}

public enum Score
{
    MAX = 1,
    MID = 2,
    MIN = 3
}
