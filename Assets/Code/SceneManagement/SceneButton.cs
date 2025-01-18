using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Button))]
[RequireComponent(typeof(SceneHandler))]
public class SceneButton : MonoBehaviour
{
    private SceneHandler sceneHandler;
    [SerializeField] private string sceneName;

#if UNITY_EDITOR
    [SerializeField] private UnityEditor.SceneAsset sceneAsset;
    private void OnValidate()
    {
        sceneHandler = GetComponent<SceneHandler>();

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
            Debug.LogError("SceneHandler not assigned");
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
