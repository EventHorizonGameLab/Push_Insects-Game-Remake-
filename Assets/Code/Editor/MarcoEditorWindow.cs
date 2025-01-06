using UnityEditor;
using UnityEngine;

public class MarcoEditorWindow : EditorWindow
{
    private string instructionsText =
        "<b>GRID CREATION TOOL (how to use)</b>\n" +
        "(<i>This will become a prefab only once it’s complete</i>)\n\n" +
        "1. Create an empty GameObject.\n" +
        "2. Reset its Transform.\n" +
        "3. Add the \"<b>Grid Generator</b>\" component.\n" +
        "4. Now the tool is ready to use.\n" +
        "5. Before creating a new grid, use the \"<b>Delete</b>\" button to remove the previous one.\n" +
        "6. <color=red>This will not work if the project is closed and reopened</color>; in that case, you need to manually delete the grid by removing the parent object.\n\n" +
        "<b>BLOCK CREATION TOOL (how to use)</b>\n\n" +
        "1. Create a Cube GameObject.\n" +
        "2. Reset its Transform.\n" +
        "3. Set the desired size (<i>insert the values in the Transform for precision</i>).\n" +
        "4. Set the Y to <b>0.55</b> if you use a normal sized Box Collider (reccomended) .\n" +
        "5. Add the \"<b>BlockHandler</b>\" script.\n" +
        "6. Now you can select the block type at any time from the script.\n" +
        "7. All the necessary components will be automatically added and configured.\n\n" +
        "<color=yellow>Important:</color> For the game to function, the Camera must have the " +
        "\"<b>Physics Raycaster</b>\" component, and the \"<b>Max Ray Interactions</b>\" parameter must be set to <b>30</b>.\n\n" +
        "<b>BUILDING A LEVEL (how to proceed)</b>\n\n" +
        "1. Create a new Scene.\n" +
        "2. Build the level using the Grid Tool (recommended <i>offsetZ = -1</i>) and the BlockHandler.\n" +
        "3. Properly set the Camera with the \"<b>CameraAligner</b>\" component (recommended <i>totalPadding = 0.5</i>).\n" +
        "4. Add an empty GameObject and assign it the \"<b>LevelData</b>\" script.\n" +
        "5. Assign a unique numeric ID to \"<b>LevelID</b>\" in the LevelData component.\n\n" +
        "<b>INITIALIZING THE GAME</b>\n\n" +
        "1. Instructions will be shown in the meeting and later documented.\n";


    // Aggiungi l'opzione al menu dell'Editor
    [MenuItem("Marco/Instructions")]
    public static void OpenWindow()
    {
        // Crea e mostra la finestra con dimensioni minime aumentate
        MarcoEditorWindow window = GetWindow<MarcoEditorWindow>("Finestra Personalizzata");
        window.maxSize = new Vector2(1000, 1000);
        window.minSize = new Vector2(1000, 1000); // Dimensioni minime
        window.Show();
    }

    // Questo metodo disegna la finestra
    private void OnGUI()
    {
        // Imposta uno stile per il testo con dimensione raddoppiata
        GUIStyle richTextStyle = new GUIStyle(GUI.skin.label)
        {
            richText = true,
            wordWrap = true,
            fontSize = 18 // Raddoppia la dimensione del carattere (default ~12)
        };

        // Aggiungi il testo formattato
        GUILayout.Label(instructionsText, richTextStyle);

        // Puoi aggiungere ulteriori controlli qui
        if (GUILayout.Button("Close Window"))
        {
            Close(); // Chiude la finestra
        }
    }
}
