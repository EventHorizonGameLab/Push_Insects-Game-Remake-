using UnityEngine;

[ExecuteInEditMode]
public class CameraAligner : MonoBehaviour
{
    public Camera targetCamera;
    public float offset = 2f;
    public float tiltAngle = 85f;

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

        float verticalFOV = targetCamera.fieldOfView * Mathf.Deg2Rad;
        float aspectRatio = targetCamera.aspect;
        float horizontalFOV = 2 * Mathf.Atan(Mathf.Tan(verticalFOV / 2) * aspectRatio);

        float distanceForWidth = (gridWidth / 2f + offset) / Mathf.Tan(horizontalFOV / 2f);
        float distanceForHeight = (gridHeight / 2f + offset) / Mathf.Tan(verticalFOV / 2f);

        float cameraDistance = Mathf.Max(distanceForWidth, distanceForHeight);

        float tiltCompensation = cameraDistance * Mathf.Tan((90f - tiltAngle) * Mathf.Deg2Rad);

        targetCamera.transform.position = new Vector3(gridCenterX, cameraDistance, gridCenterZ - tiltCompensation);

        targetCamera.transform.rotation = Quaternion.Euler(tiltAngle, 0, 0);
    }
}
