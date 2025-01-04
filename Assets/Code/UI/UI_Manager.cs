using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static Action OnUpdateMoves;

    [Header("Panels")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject levelMenuPanel;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject inGameView;
    [Header("Texts")]
    [SerializeField] TMP_Text record_txt;
    [SerializeField] TMP_Text currentMoves;

    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void Start()
    {
        
    }

    void Init()
    {
        mainMenuPanel.SetActive(true);
        levelMenuPanel.SetActive(false);
        inGameView.SetActive(false);
        optionPanel.SetActive(false);
        record_txt.text = string.Empty;
        currentMoves.text = string.Empty;
    }
}  
