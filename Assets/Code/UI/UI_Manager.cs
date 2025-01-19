using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;


public class UI_Manager : MonoBehaviour
{
    public static Action<int, int> OnUpdateMoves;
    public static Action<LevelData> OnGivingGameUI;
    public static Action<Score, int> OnWinScreen;
    public static event Action OnRequestingMenu;
    public static event Action OnRequestingNextLevel;
    public static event Action OnRequestingLastScene;

    [Header("Panels")]
    [SerializeField] RectTransform mainMenuPanel;
    [SerializeField] List<RectTransform> allScreens;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject inGameView;
    [Header("Texts")]
    [SerializeField] TMP_Text record_txt;
    [SerializeField] TMP_Text currentMoves_txt;
    [SerializeField] TMP_Text levelID_txt;
    [Header("Sprites")]
    [SerializeField] List<Sprite> difficulty_sprites;
    [SerializeField] List<Sprite> Predator_sprites;
    [Header("Image Field")]
    [SerializeField] Image difficulty_img;
    [SerializeField] Image predator_img;
    [Header("Endgame")]
    [SerializeField] RectTransform winScreen;
    [SerializeField] List<RectTransform> seeds;
    [SerializeField] TMP_Text finalMoves_txt;

    //---\\
    Vector3 mainScreenPos;

    List<Vector2> initialScreenPos = new List<Vector2>();



    private void Awake()
    {
        InitMain();
    }
    private void OnEnable()
    {
        OnGivingGameUI += InitGameUI;
        OnUpdateMoves += UpdateMoves;
        OnRequestingMenu += InitMain;
        OnWinScreen += ShowScore;
    }
    private void OnDisable()
    {
        OnGivingGameUI -= InitGameUI;
        OnUpdateMoves -= UpdateMoves;
        OnRequestingMenu -= InitMain;
        OnWinScreen -= ShowScore;
    }

    void InitMain()
    {
        SaveInitialScreensPosition();
        mainScreenPos = mainMenuPanel.anchoredPosition;
        mainMenuPanel.gameObject.SetActive(true);
        inGameView.SetActive(false);
        optionPanel.SetActive(false);
        winScreen.gameObject.SetActive(false);
        record_txt.text = string.Empty;
        currentMoves_txt.text = string.Empty;
    }

    void InitGameUI(LevelData levelData)
    {
        DeactivateMenus();
        inGameView.SetActive(true); // activate UI game only
        record_txt.text = levelData.GetRecord("record").ToString();
        levelID_txt.text = levelData.levelID.ToString();
        currentMoves_txt.text = "0";
        HandleActivationList(seeds, false);
        difficulty_img.sprite = GetDifficultyImage(levelData.difficulty);
        predator_img.sprite = GetPredatorImage(levelData.difficulty);
    }




    void UpdateMoves(int moves, int record)
    {
        currentMoves_txt.text = moves.ToString();
        record_txt.text = record != 0 ? record.ToString() : record_txt.text;
    }

    void ShowScore(Score score, int moves)
    {
        winScreen.gameObject.SetActive(true);
        finalMoves_txt.text = moves.ToString();
        for (int i = 0; i <= (int)score; i++)
        {
            seeds[i].gameObject.SetActive(true);
        }
    }

    Sprite GetDifficultyImage(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.BEGINNER: return difficulty_sprites[0];
            case Difficulty.INTERMEDIATE: return difficulty_sprites[1];
            case Difficulty.ADVANCED: return difficulty_sprites[2];
            default: return null;
        }
    }

    Sprite GetPredatorImage(Difficulty difficulty)
    {
        if (Predator_sprites.Count == 0) return null;
        switch (difficulty)
        {
            case Difficulty.BEGINNER: return Predator_sprites[0];
            case Difficulty.INTERMEDIATE: return Predator_sprites[1];
            case Difficulty.ADVANCED: return Predator_sprites[2];
            default: return null;
        }
    }

    public void HandleActivationList(List<GameObject> activaionList, bool value) // available for buttons
    {
        foreach (GameObject obj in activaionList) obj.SetActive(value);
    }
    void HandleActivationList(List<RectTransform> activaionList, bool value)
    {
        foreach (RectTransform obj in activaionList) obj.gameObject.SetActive(value);
    }

    void SaveInitialScreensPosition()
    {
        for (int i = 0; i < allScreens.Count; i++)
        {
            initialScreenPos.Add(allScreens[i].anchoredPosition);
            Debug.Log(allScreens[i].anchoredPosition);
        }
    }

    void RestoreScreenPositions()
    {
        for (int i = 0; i < allScreens.Count; i++)
        {
            allScreens[i].anchoredPosition = initialScreenPos[i];
            allScreens[i].gameObject.SetActive(true);
        }
    }

    void DeactivateMenus()
    {
        mainMenuPanel.gameObject.SetActive(false);
        optionPanel.gameObject.SetActive(false);
        foreach (RectTransform r in allScreens) r.gameObject.SetActive(false);
    }



    //-- Buttons Calls --\\

    #region ButtonsCalls
    public void RequestMainMenu() //for button
    {
        HandleActivationList(seeds, false);
        winScreen.gameObject.SetActive(false);
        OnRequestingMenu();
        RestoreScreenPositions();
    }

    public void RequestNextLevel()
    {
        HandleActivationList(seeds, false);
        winScreen.gameObject.SetActive(false);
        OnRequestingNextLevel();
    }

    public void RequestLastScene()
    {
        OnRequestingLastScene();
        CameraHandler.OnLevelLoaded?.Invoke();
    }

    public void GoToEasyLevelsScreen()
    {
        foreach (RectTransform screen in allScreens)
        {
            screen.DOAnchorPos(screen.anchoredPosition + Vector2.up * 1920, 0.6f);
        }
    }

    public void GoToIntermediateLevelsScreen()
    {
        foreach (RectTransform screen in allScreens)
        {
            screen.DOAnchorPos(screen.anchoredPosition + Vector2.up * 1920 * 2, 0.6f);
        }
    }

    public void GoToAdvancedLevelsScreen()
    {
        foreach (RectTransform screen in allScreens)
        {
            screen.DOAnchorPos(screen.anchoredPosition + Vector2.up * 1920 * 3, 0.6f);
        }
    }

    public void GoToSkinScreen()
    {
        foreach (RectTransform screen in allScreens)
        {
            screen.DOAnchorPos(screen.anchoredPosition + Vector2.up * -1920, 0.6f);
        }
    }

    public void SlideToMain(float tweenValue)
    {
        foreach (RectTransform screen in allScreens)
        {
            screen.DOAnchorPos(screen.anchoredPosition + Vector2.up * tweenValue, 0.6f);
        }
    }

    #endregion

}