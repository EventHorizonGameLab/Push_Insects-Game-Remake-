using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class UI_Manager : MonoBehaviour
{
    public static Action<int, int> OnUpdateMoves;
    public static Action<LevelData> OnGivingGameUI;
    public static event Action OnRequestingMenu;

    [Header("Panels")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject levelMenuPanel;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject inGameView;
    [Header("Buttons")]
    [SerializeField] GameObject backButton;
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
        mainMenuPanel.SetActive(true);
        levelMenuPanel.SetActive(false);
        inGameView.SetActive(false);
        optionPanel.SetActive(false);
        record_txt.text = string.Empty;
        currentMoves_txt.text = string.Empty;
    }

    void InitGameUI(LevelData levelData)
    {
        mainMenuPanel.SetActive(false);
        levelMenuPanel.SetActive(false);
        inGameView.SetActive(true); // activate UI game only
        optionPanel.SetActive(false);
        backButton.SetActive(false);
        record_txt.text = levelData.GetRecord("record").ToString();
        levelID_txt.text = levelData.levelID.ToString();
        currentMoves_txt.text = "0";
        difficulty_img.sprite = GetDifficultyImage(levelData.difficulty);
    }

    void UpdateMoves(int moves, int record)
    {
        currentMoves_txt.text = moves.ToString();
        record_txt.text = record.ToString();
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

}
