using UnityEngine;

[ExecuteInEditMode]
public class CameraAligner : MonoBehaviour
{
    public Camera targetCamera;
    public float totalPadding = 2f;

    public void AlignCameraToGrid(int rows, int columns, float cubeSize)
    {
        if (targetCamera == null)
        {
            Debug.LogError("Devi assegnare una camera!");
            return;
        }

        float gridWidth = columns * cubeSize;
        float gridHeight = rows * cubeSize;

        float gridCenterX = (columns - 1) * cubeSize / 2f;
        float gridCenterZ = (rows - 1) * cubeSize / 2f;

        if (targetCamera.orthographic)
        {
            // Dimensione ortografica calcolata in base all'altezza
            targetCamera.orthographicSize = (gridHeight / 2f) + totalPadding;

            // Calcolo dell'aspect ratio della camera per la larghezza
            float cameraWidth = targetCamera.orthographicSize * targetCamera.aspect;
            if (cameraWidth < (gridWidth / 2f) + totalPadding)
            {
                // Se la larghezza non è sufficiente, ricalcolo il size per includere la larghezza
                targetCamera.orthographicSize = ((gridWidth / 2f) + totalPadding) / targetCamera.aspect;
            }

            // Posizionamento della camera sopra la griglia
            targetCamera.transform.position = new Vector3(gridCenterX, 10f, gridCenterZ); // Altezza 10 come esempio
            targetCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Vista dall'alto
        }
        else
        {
            Debug.LogWarning("La camera non è impostata su ortografica!");
        }
    }
}
