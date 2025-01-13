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
    public static event Action OnRequestingMenu;

    [Header("Panels")]
    [SerializeField] RectTransform mainMenuPanel;
    [SerializeField] List<RectTransform> allScreens;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject inGameView;
    [Header("Buttons")]
    [SerializeField] List<GameObject> mainButtons;
    [SerializeField] List<GameObject> difficultyButtons;
    [Header("Texts")]
    [SerializeField] TMP_Text record_txt;
    [SerializeField] TMP_Text currentMoves_txt;
    [SerializeField] TMP_Text levelID_txt;
    [Header("Sprites")]
    [SerializeField] Sprite beginner_sprite;
    [SerializeField] Sprite intermediate_sprite;
    [SerializeField] Sprite advanced_sprite;
    [Header("Image Field")]
    [SerializeField] Image difficulty_img;
    [Header("Endgame")]
    [SerializeField] RectTransform winScreen;
    [SerializeField] List<RectTransform> seeds;

    //---\\
    Vector3 mainScreenPos;



    private void Awake()
    {
        InitMain();
    }
    private void OnEnable()
    {
        OnGivingGameUI += InitGameUI;
        OnUpdateMoves += UpdateMoves;
        OnRequestingMenu += InitMain;
    }
    private void OnDisable()
    {
        OnGivingGameUI -= InitGameUI;
        OnUpdateMoves -= UpdateMoves;
        OnRequestingMenu -= InitMain;
    }
    private void Start()
    {

    }

    void InitMain()
    {
        mainScreenPos = mainMenuPanel.anchoredPosition;
        mainMenuPanel.gameObject.SetActive(true);
        inGameView.SetActive(false);
        optionPanel.SetActive(false);
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
        difficulty_img.sprite = GetDifficultyImage(levelData.difficulty);
        
        //TODO: GET PREDATOR IMAGE
    }

    void UpdateMoves(int moves, int record)
    {
        currentMoves_txt.text = moves.ToString();
        record_txt.text = record != 0 ? record.ToString() : record_txt.text;
    }

    public void RequestMainMenu() //for button
    {
        OnRequestingMenu();
    }

    Sprite GetDifficultyImage(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.BEGINNER: return beginner_sprite;
            case Difficulty.INTERMEDIATE: return intermediate_sprite;
            case Difficulty.ADVANCED: return advanced_sprite;
            default: return null;
        }
    }

    public void HandleActivaionList(List<GameObject> activaionList, bool value)
    {
        foreach (GameObject obj in activaionList) obj.SetActive(value);
    }

    void DeactivateMenus()
    {
        mainMenuPanel.gameObject.SetActive(false);
        optionPanel.gameObject.SetActive(false);
        foreach(RectTransform r in allScreens) r.gameObject.SetActive(false);
    }

    //-- Buttons Calls --\\

    #region ButtonsCalls

    public void GoToEasyLevelsScreen()
    {
        foreach(RectTransform screen in allScreens )
        {
            screen.DOAnchorPos(screen.anchoredPosition + Vector2.up * 1920, 0.5f);
        }
    }

    public void GoToIntermediateLevelsScreen()
    {
        foreach (RectTransform screen in allScreens)
        {
            screen.DOAnchorPos(screen.anchoredPosition + Vector2.up * 1920 * 2, 0.5f);
        }
    }

    public void GoToAdvancedLevelsScreen()
    {
        foreach (RectTransform screen in allScreens)
        {
            screen.DOAnchorPos(screen.anchoredPosition + Vector2.up * 1920 * 3, 0.5f);
        }
    }

    #endregion

}
