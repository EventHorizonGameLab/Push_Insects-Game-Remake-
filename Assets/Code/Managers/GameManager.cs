using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action OnStartingGame;

    public static Action OnMoveMade;

    public static Action<bool> OnPlayerDragging;


    bool playerIsDragging;

    LevelData levelData;

    int currentMovesCount;
    int currentRecord;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(gameObject);
        
        currentMovesCount = 0;
        Application.targetFrameRate = 60;
    }


    private void OnEnable()
    {
        OnStartingGame += StartLevel;
        OnPlayerDragging += ChangePlayerState;
        OnMoveMade += UpdateLevelData;
    }
    private void OnDisable()
    {
        OnPlayerDragging -= ChangePlayerState;
        OnStartingGame -= StartLevel;
        OnMoveMade -= UpdateLevelData;
    }
    public bool PlayerIsDragging() => playerIsDragging;

    void ChangePlayerState(bool state)
    {
        playerIsDragging = state;
    }

    void StartLevel()
    {
        levelData = FindAnyObjectByType<LevelData>();
        if(levelData == null) return;
        currentRecord = levelData.GetRecord("record");
        UI_Manager.OnGivingGameUI(levelData);
    }

    void UpdateLevelData()
    {
        currentMovesCount++;
        if (currentMovesCount > currentRecord)
        {
            currentRecord = currentMovesCount;
            levelData.SaveRecord("record", currentRecord);
        }

        UI_Manager.OnUpdateMoves?.Invoke(currentMovesCount, currentRecord);
    }

    void EndLevel()
    {
        levelData = null;
        currentMovesCount = 0;
        currentRecord = 0;
        // TODO: show score
    }
}
