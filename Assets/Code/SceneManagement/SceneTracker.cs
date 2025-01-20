using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneTracker : MonoBehaviour
{
    public static Action<string> OnLoadNextLevel;
    public static Action<string> OnLoadTrackedLevel;
    public static event Action<bool> OnLevelListEnded;

    [SerializeField] SceneHandler sceneHandler;
    [SerializeField] List<string> sceneNames = new List<string>();


#if UNITY_EDITOR
    [SerializeField] List<UnityEditor.SceneAsset> allScenes;

    private void OnValidate()
    {
        if (allScenes != null)
        {
            sceneNames.Clear();
            foreach (var scene in allScenes)
            {
                sceneNames.Add(scene.name);
            }
        }
    }
#endif

    private void OnEnable()
    {
        OnLoadNextLevel += LoadNextLevel;
        OnLoadTrackedLevel += LoadLastPlayed;
    }
    private void OnDisable()
    {
        OnLoadNextLevel -= LoadNextLevel;
        OnLoadTrackedLevel -= LoadLastPlayed;
    }

    void LoadNextLevel(string sceneName)
    {
        Debug.Log($"loading scene{sceneName}");
        int currentIndex = sceneNames.IndexOf(sceneName);
        if (currentIndex + 1 < sceneNames.Count)
        {
            var nextSceneName = sceneNames[currentIndex + 1];
            sceneHandler.UnloadSceneFromName(sceneName);
            sceneHandler.LoadSceneFromName(nextSceneName);

            int nextIndex = sceneNames.IndexOf(nextSceneName);
            if(nextIndex + 1 > sceneNames.Count) OnLevelListEnded(true);
        }
        else
        {
            UI_Manager ui = GetComponent<UI_Manager>();
            ui.RequestMainMenu();
            Debug.Log("All Levels Played");
            return;
        }
    }

    private void LoadLastPlayed(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            sceneHandler.LoadSceneFromName(sceneNames[0]);
        }
        else
        {
            sceneHandler.LoadSceneFromName(sceneName);
        }
        
    }




}
