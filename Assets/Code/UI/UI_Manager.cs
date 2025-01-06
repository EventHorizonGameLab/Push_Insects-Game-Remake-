using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static Action<int, int> OnUpdateMoves;
    public static Action<LevelData> OnGivingGameUI;

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

    private void Awake()
    {
        InitMain();
    }
    private void OnEnable()
    {
        OnGivingGameUI += InitGameUI;
        OnUpdateMoves += UpdateMoves;
    }
    private void OnDisable()
    {
        OnGivingGameUI -= InitGameUI;
        OnUpdateMoves -= UpdateMoves;
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
        currentMoves_txt.text = "0"; 
    }

    void UpdateMoves(int moves, int record)
    {
        currentMoves_txt.text = moves.ToString();
        record_txt.text = record.ToString();
    }
    
}  
