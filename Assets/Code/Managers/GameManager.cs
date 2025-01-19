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

   // public static Action OnGoingNextLevel;

    public static Action<bool> OnPlayerDragging;

    [SerializeField] Score_Manager scoreManager;


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
        OnLevelCompleted += EndLevel;
        UI_Manager.OnRequestingMenu += ReloadMainMenu;
        UI_Manager.OnRequestingNextLevel += GoToNextLevel;
        UI_Manager.OnRequestingLastScene += LoadSceneByStart;

    }
    private void OnDisable()
    {
        OnPlayerDragging -= ChangePlayerState;
        OnStartingGame -= StartLevel;
        OnMoveMade -= UpdateLevelData;
        OnMoveUndone -= UpdateLevelDataOnUndo;
        OnLevelCompleted -= EndLevel;
        UI_Manager.OnRequestingMenu -= ReloadMainMenu;
        UI_Manager.OnRequestingNextLevel -= GoToNextLevel;
        UI_Manager.OnRequestingLastScene -= LoadSceneByStart;
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
        SaveLastScene(lastActiveScene);
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

    void EndLevel()//minimodificaByEma
    {
        Score finalScore = scoreManager.CalculateScore(levelData, currentMovesCount);

        if (levelData != null)
        {
            levelData.CompleteLevel(currentMovesCount);
        }
        UI_Manager.OnWinScreen?.Invoke(finalScore, currentMovesCount);
    }

    void GoToNextLevel()
    {
        levelData.SaveRecord("record", currentRecord);
        levelData = null;
        currentMovesCount = 0;
        currentRecord = 0;
        SceneTracker.OnLoadNextLevel(lastActiveScene);
    }

    void ReloadMainMenu()
    {
        if (!string.IsNullOrEmpty(lastActiveScene) && SceneManager.GetSceneByName(lastActiveScene).isLoaded)
        {
            SceneHandler.OnUnloading?.Invoke(lastActiveScene);
        }

        levelData = null;
        currentMovesCount = 0;
        currentRecord = 0;
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

    void SaveLastScene(string sceneName)
    {
        PlayerPrefs.SetString("LastScene", sceneName);
        PlayerPrefs.Save();
    }

    string GetLastScene()
    {
        return PlayerPrefs.GetString("LastScene");
    }

    void LoadSceneByStart()
    {
        Debug.Log(GetLastScene());
        SceneTracker.OnLoadTrackedLevel?.Invoke(GetLastScene());
    }
}
