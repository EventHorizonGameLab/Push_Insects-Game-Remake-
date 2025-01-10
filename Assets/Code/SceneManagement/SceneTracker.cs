using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneTracker : MonoBehaviour
{
    public static Action<string> OnLoadNextLevel;

    [SerializeField] SceneHandler sceneHandler;
    List<string> sceneNames = new List<string>();


#if UNITY_EDITOR
    [SerializeField] List<SceneAsset> allScenes;

    private void OnValidate()
    {
        if (allScenes != null)
        {
            sceneNames.Clear();
            foreach (SceneAsset scene in allScenes)
            {
                sceneNames.Add(scene.name);
            }
        }
    }
#endif

    private void OnEnable()
    {
        OnLoadNextLevel += LoadNextLevel;
    }
    private void OnDisable()
    {
        OnLoadNextLevel -= LoadNextLevel;
    }

    void LoadNextLevel(string sceneName)
    {
        int currentIndex = sceneNames.IndexOf(sceneName);
        if (currentIndex + 1 < sceneNames.Count)
        {
            var nextSceneName = sceneNames[currentIndex + 1];
            sceneHandler.LoadSceneFromName(nextSceneName);
        }
        else
        {
            Debug.Log("No scene available");
            return;
        }

        
    }


}
