using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class SceneButton : MonoBehaviour
{
    [SerializeField] private SceneHandler sceneHandler;
    [SerializeField] private string sceneName;

#if UNITY_EDITOR
    [SerializeField] private UnityEditor.SceneAsset sceneAsset;
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif

    private void Awake()
    {
        if (sceneHandler == null)
        {
            Debug.LogError("SceneHandler non assegnato al pulsante!");
            return;
        }

        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => {
                sceneHandler.LoadSceneFromName(sceneName); 
                CameraHandler.OnLevelLoaded?.Invoke();
     
            });
        }
    }
}
