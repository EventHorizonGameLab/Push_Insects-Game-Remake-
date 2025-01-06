using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static Action OnSceneLoaded;

    [SerializeField] private GameObject loadingImage;

    private void Awake()
    {
        if (loadingImage != null)
        {
            loadingImage.SetActive(false);
        }
    }

    /// <summary>
    /// Funzione chiamabile da un bottone, accetta il nome della scena da caricare e la carica in modo additivo.
    /// </summary>
    /// <param name="sceneName">Il nome della scena da caricare.</param>
    public void LoadSceneFromName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Nome della scena non valido!");
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

        if (loadingImage != null)
        {
            loadingImage.SetActive(false);
        }

        GameManager.OnStartingGame();
    }
}
