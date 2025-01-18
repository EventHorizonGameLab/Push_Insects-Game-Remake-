using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static Action OnSceneLoaded;
    public static Action<string> OnSpecificLoad;
    public static Action<string> OnUnloading;

    [SerializeField] private GameObject loadingImage;

    private string currentScene = string.Empty;
    private bool isLoading = false;

    private void Awake()
    {
        if (loadingImage != null)
        {
            loadingImage.SetActive(false);
        }
    }

    private void OnEnable()
    {
        OnSpecificLoad += LoadLastPlayed;
        OnUnloading += UnloadSceneFromName;
    }

    private void OnDisable()
    {
        OnSpecificLoad -= LoadLastPlayed;
        OnUnloading -= UnloadSceneFromName;
    }

    public void LoadSceneFromName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Not valid scene name");
            return;
        }

        if (isLoading)
        {
            Debug.LogWarning("A scene is already loading");
            return;
        }

        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    public void UnloadSceneFromName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[UnloadSceneFromName] Nome della scena non valido o nullo.");
            return;
        }

        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            Debug.LogError($"[UnloadSceneFromName] La scena '{sceneName}' non è caricata.");
            return;
        }

        if (isLoading)
        {
            Debug.LogWarning("[UnloadSceneFromName] Una scena è già in fase di caricamento/scaricamento.");
            return;
        }

        StartCoroutine(UnloadSceneCoroutine(sceneName));
    }


    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        isLoading = true;

        if (loadingImage != null)
        {
            loadingImage.SetActive(true);
        }

        // Verifica e scarica la scena corrente
        if (!string.IsNullOrEmpty(currentScene))
        {
            var sceneToUnload = SceneManager.GetSceneByName(currentScene);
            if (sceneToUnload.IsValid() && sceneToUnload.isLoaded)
            {
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentScene);
                while (!asyncUnload.isDone)
                {
                    yield return null;
                }
                Debug.Log($"[LoadSceneCoroutine] Scena scaricata: {currentScene}");
            }
            else
            {
                Debug.LogWarning($"[LoadSceneCoroutine] La scena '{currentScene}' non è valida o non è caricata.");
            }
        }

        // Caricamento della nuova scena
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        currentScene = sceneName;

        if (loadingImage != null)
        {
            loadingImage.SetActive(false);
        }

        isLoading = false;

        OnSceneLoaded?.Invoke();
        GameManager.OnStartingGame();
    }

    private IEnumerator UnloadSceneCoroutine(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name cannot be null or empty.");
            yield break;
        }

        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            Debug.LogWarning($"Scene '{sceneName}' is not loaded.");
            yield break;
        }

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        float unloadTimeout = 10f; // Timeout di 10 secondi per la rimozione
        while (!asyncUnload.isDone)
        {
            yield return null;
            unloadTimeout -= Time.deltaTime;
            if (unloadTimeout <= 0f)
            {
                Debug.LogError("Timeout durante lo scaricamento della scena.");
                break;
            }
        }

        if (asyncUnload.isDone)
        {
            Debug.Log($"Scene '{sceneName}' unloaded successfully.");
        }
        else
        {
            Debug.LogError($"Errore durante lo scaricamento della scena: {sceneName}");
        }
    }


    private void LoadLastPlayed(string sceneName)
    {
        
        LoadSceneFromName(sceneName);
    }
}
