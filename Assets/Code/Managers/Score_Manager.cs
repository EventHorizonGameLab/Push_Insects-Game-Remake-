
using UnityEngine;

public class Score_Manager : MonoBehaviour
{
    
    public Score CalculateScore(LevelData levelData, int movesMade) 
    {
        if (movesMade <= levelData.movesForMaxScore) return Score.MAX;
        if (movesMade <= levelData.movesForMidScore) return Score.MID;
        return Score.MIN;
    }
}

public enum Score
{
    MIN,
    MID,
    MAX
}
