using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action OnStartingGame;

    public static Action OnMoveMade;

    public static Action<IBlock, Vector3> OnMoveToRegister;

    public static Action OnMoveUndone;

    public static Action OnLevelCompleted;

    public static Action OnGoingNextLevel; //TODO: IMPLEMENTARE DOPO IMPLEMENTAZIONE SCORE

    public static Action<bool> OnPlayerDragging;


    bool playerIsDragging;

    LevelData levelData;
    string lastActiveScene;

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
        OnMoveUndone += UpdateLevelDataOnUndo;
        OnLevelCompleted += GoToNextLevel; //TODO: DEVE DINVETARE SHOWSCORE
        UI_Manager.OnRequestingMenu += ReloadMainMenu;

    }
    private void OnDisable()
    {
        OnPlayerDragging -= ChangePlayerState;
        OnStartingGame -= StartLevel;
        OnMoveMade -= UpdateLevelData;
        OnMoveUndone -= UpdateLevelDataOnUndo;
        OnLevelCompleted -= GoToNextLevel;
        UI_Manager.OnRequestingMenu -= ReloadMainMenu;

    }
    public bool PlayerIsDragging() => playerIsDragging;

    void ChangePlayerState(bool state)
    {
        playerIsDragging = state;
    }

    void StartLevel()
    {
        levelData = FindAnyObjectByType<LevelData>();
        if (levelData == null) return;
        currentRecord = levelData.GetRecord("record");
        lastActiveScene = levelData.gameObject.scene.name;
        UI_Manager.OnGivingGameUI(levelData);
    }

    void UpdateLevelData()
    {
        currentMovesCount++;

        if (currentMovesCount > currentRecord)
        {
            currentRecord = currentMovesCount;
        }

        UI_Manager.OnUpdateMoves?.Invoke(currentMovesCount, currentRecord);
    }

    void UpdateLevelDataOnUndo()
    {
        currentMovesCount--;

        int savedRecord = levelData.GetRecord("record");
        currentRecord = Mathf.Max(savedRecord, currentMovesCount);

        UI_Manager.OnUpdateMoves?.Invoke(currentMovesCount, currentRecord);
    }
   


    void EndLevel()
    {
        levelData.SaveRecord("record", currentRecord);
        levelData = null;
        currentMovesCount = 0;
        currentRecord = 0;
        // TODO: show score
    }

    void GoToNextLevel()
    {
        EndLevel(); // DEBUG ONLY
        SceneTracker.OnLoadNextLevel(lastActiveScene);
    }

    void ReloadMainMenu()
    {
        if (!string.IsNullOrEmpty(lastActiveScene) && SceneManager.GetSceneByName(lastActiveScene).isLoaded)
        {
            SceneHandler.OnUnloading?.Invoke(lastActiveScene);
        }

        EndLevel();
        CameraHandler.OnMenuLoaded?.Invoke();
        UI_Manager.OnUpdateMoves?.Invoke(0, currentRecord); // impedisce il salvataggio se si esce dal livello

        // Aspetta che la scena venga completamente scaricata prima di resettare
        StartCoroutine(ResetSceneState());
    }

    private IEnumerator ResetSceneState()
    {
        while (SceneManager.GetSceneByName(lastActiveScene).isLoaded)
        {
            yield return null; // Aspetta che la scena venga completamente scaricata
        }

        lastActiveScene = null;
        levelData = null;
    }

}
