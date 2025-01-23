using System;
using System.Collections;
using DG.Tweening;
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
    bool gameIsCompleted;

    LevelData levelData;
    string lastActiveScene;

    int currentMovesCount;
    int currentRecord;
    int temporaryRecord;

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
        OnMoveMade += UpdateUI;
        OnMoveUndone += UpdateUIUndo;
        OnLevelCompleted += EndLevel;
        UI_Manager.OnRequestingMenu += ReloadMainMenu;
        UI_Manager.OnRequestingNextLevel += GoToNextLevel;
        UI_Manager.OnRequestingLastScene += LoadSceneByStart;
        SceneTracker.OnLevelListEnded += SetGameAsCompleted;

    }
    private void OnDisable()
    {
        OnPlayerDragging -= ChangePlayerState;
        OnStartingGame -= StartLevel;
        OnMoveMade -= UpdateUI;
        OnMoveUndone -= UpdateUIUndo;
        OnLevelCompleted -= EndLevel;
        UI_Manager.OnRequestingMenu -= ReloadMainMenu;
        UI_Manager.OnRequestingNextLevel -= GoToNextLevel;
        UI_Manager.OnRequestingLastScene -= LoadSceneByStart;
        SceneTracker.OnLevelListEnded -= SetGameAsCompleted;
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

    void UpdateUI()
    {
        currentMovesCount++;

        if (currentRecord == 0 || temporaryRecord < currentRecord) temporaryRecord = currentMovesCount;

        if (currentRecord != 0 && temporaryRecord > currentRecord) temporaryRecord = currentRecord;
        

        int valueToShow = currentRecord > temporaryRecord ? currentRecord : temporaryRecord;
        UI_Manager.OnUpdateMoves?.Invoke(currentMovesCount, valueToShow);
        Debug.Log("il valore attuale é" +temporaryRecord);
    }

    void UpdateLevelData()
    {
        if (currentRecord > 0)
            temporaryRecord = temporaryRecord < currentRecord ? temporaryRecord : currentRecord;
        
        levelData.SaveRecord("record", temporaryRecord);
    }

    void UpdateUIUndo()
    {
        currentMovesCount--;

        temporaryRecord = (int)MathF.Max(currentMovesCount, currentRecord);
        UI_Manager.OnUpdateMoves?.Invoke(currentMovesCount, temporaryRecord);
        Debug.Log($"il valore salvato al momento é:{currentRecord} mentre il temporary é {temporaryRecord}");
    }

    void EndLevel()//minimodificaByEma
    {
        if (currentRecord == 0 || temporaryRecord < currentRecord) temporaryRecord = currentMovesCount;

        if (currentRecord != 0 && temporaryRecord > currentRecord) temporaryRecord = currentRecord;


        int valueToShow = currentRecord > temporaryRecord ? currentRecord : temporaryRecord;
        UI_Manager.OnUpdateMoves?.Invoke(currentMovesCount, valueToShow);
        UpdateLevelData();
        temporaryRecord = 0;
        
        Score finalScore = scoreManager.CalculateScore(levelData, currentMovesCount);

        if (levelData != null)
        {
            levelData.CompleteLevel(currentMovesCount);
        }
        UI_Manager.OnWinScreen?.Invoke(finalScore, currentMovesCount);
    }

    void GoToNextLevel()
    {
        if (gameIsCompleted)
        {
            Debug.Log(gameIsCompleted);
            ReloadMainMenu();
            return;
        }
        else
        {
            
            levelData = null;
            currentMovesCount = 0;
            currentRecord = 0;
            SceneTracker.OnLoadNextLevel(lastActiveScene);
        }
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
        SceneTracker.OnLoadTrackedLevel?.Invoke(GetLastScene());
    }

    void SetGameAsCompleted(bool value) => gameIsCompleted = value;
}
