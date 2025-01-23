using UnityEngine;
using UnityEngine.UI;

public class PierCode : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private DifficultyProgress difficultyProgress; // Collegare lo ScriptableObject della categoria
    [SerializeField] private float maxTimeBetweenClicks = 0.5f; // Massimo tempo tra i click
    [SerializeField] private int requiredClicks = 3; // Numero di click richiesti

    [Header("UI Components")]
    [SerializeField] private Button button; // Collegare il pulsante che attiva lo sblocco

    private int clickCount = 0;
    private float lastClickTime;

    private void Start()
    {
        if (button == null)
        {
            Debug.LogError("Button not assigned!");
            return;
        }

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }

    private void OnButtonClicked()
    {
        float currentTime = Time.time;

        // Se il tempo trascorso dall'ultimo click supera il limite, resettare il conteggio
        if (currentTime - lastClickTime > maxTimeBetweenClicks)
        {
            clickCount = 0;
        }

        clickCount++;
        lastClickTime = currentTime;

        if (clickCount >= requiredClicks)
        {
            UnlockAllLevelButtons();
            clickCount = 0; // Resetta il conteggio
        }
    }

    private void UnlockAllLevelButtons()
    {
        if (difficultyProgress == null)
        {
            Debug.LogError("DifficultyProgress not assigned!");
            return;
        }

        // Trova tutti i LevelButtonHandler nella scena
        var levelButtonHandlers = Object.FindObjectsByType<LevelButtonHandler>(FindObjectsSortMode.None);

        foreach (var buttonHandler in levelButtonHandlers)
        {
            // Controlla che il pulsante appartenga alla stessa categoria di difficoltà
            if (buttonHandler != null && buttonHandler.DifficultyProgress == difficultyProgress)
            {
                // Ottieni il livello associato dal DifficultyProgress
                var levelData = difficultyProgress.levels.Find(l => l.levelID == buttonHandler.levelID);

                if (levelData != null)
                {
                    // Abilita il pulsante solo se il livello non è completato
                    if (!levelData.isCompleted)
                    {
                        buttonHandler.EnableButton(true); // Abilita il pulsante
                        buttonHandler.UpdateButtonVisual(0); // Mostra zero stelle per i livelli sbloccati segretamente
                    }
                    else
                    {
                        // Mantieni il pulsante abilitato con i progressi originali
                        buttonHandler.EnableButton(true);
                        buttonHandler.UpdateButtonVisual(levelData.starsEarned); // Ripristina le stelle originali
                    }
                }
            }
        }

        Debug.Log($"All level buttons for '{difficultyProgress.name}' have been unlocked!");
    }
}
