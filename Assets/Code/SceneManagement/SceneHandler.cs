using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static Action OnSceneLoaded;
    public static Action<string> OnSpecificLoad;

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
    }

    private void OnDisable()
    {
        OnSpecificLoad -= LoadLastPlayed;
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

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        isLoading = true;

        if (loadingImage != null)
        {
            loadingImage.SetActive(true);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (!string.IsNullOrEmpty(currentScene) && SceneManager.GetSceneByName(currentScene).isLoaded)
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

        isLoading = false;

        OnSceneLoaded?.Invoke();
        GameManager.OnStartingGame();
    }

    private void LoadLastPlayed(string sceneName)
    {
        LoadSceneFromName(sceneName);
    }
}
