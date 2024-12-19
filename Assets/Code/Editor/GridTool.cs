using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
public class GridTool : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridGenerator script = (GridGenerator)target;

        if (GUILayout.Button("Generate Grid"))
        {
            script.GenerateGrid();
        }

        if (GUILayout.Button("Delete Grid"))
        {
            script.DeleteGrid();
        }
    }
}
