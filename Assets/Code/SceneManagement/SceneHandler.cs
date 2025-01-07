using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static Action OnSceneLoaded;

    public static Action<string> OnSpecificLoad;

    [SerializeField] private GameObject loadingImage;

    string currentScene = string.Empty;

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
    }
    private void OnDisable()
    {
        OnSpecificLoad -= LoadLastPlayed;
    }

    /// <summary>
    /// Funzione chiamabile da un bottone, accetta il nome della scena da caricare e la carica in modo additivo.
    /// </summary>
    /// <param name="sceneName">Il nome della scena da caricare.</param>
    public void LoadSceneFromName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Not valid scene name");
            return;
        }

        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    /// <summary>
    /// Coroutine per caricare la scena e gestire l'immagine di caricamento.
    /// </summary>
    /// <param name="sceneName">Il nome della scena da caricare.</param>
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        if (loadingImage != null)
        {
            loadingImage.SetActive(true);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (currentScene != string.Empty)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentScene);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }

        currentScene = sceneName;

        if (loadingImage != null)
        {
            loadingImage.SetActive(false);
        }

        GameManager.OnStartingGame();
    }

    void LoadLastPlayed(string sceneName)
    {
        LoadSceneFromName(sceneName);
    }

}
