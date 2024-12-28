using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action<bool> OnPlayerDragging;

    bool playerIsDragging;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(gameObject);
    }


    private void OnEnable()
    {
        OnPlayerDragging += ChangePlayerState;
    }
    private void OnDisable()
    {
        OnPlayerDragging -= ChangePlayerState;
    }
    public bool PlayerIsDragging() => playerIsDragging;

    void ChangePlayerState(bool state)
    {
        playerIsDragging = state;
    }
}
