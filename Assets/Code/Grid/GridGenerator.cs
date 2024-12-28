using UnityEngine;

[ExecuteInEditMode]
public class GridGenerator : MonoBehaviour
{
    public int rows = 5;
    public int columns = 5;
    public float cubeSize = 1f;
    public float scaleY = 1f;
    public float offsetZ = 0f; // Nuova variabile per l'offset Z
    public bool singleCollider = false;

    public Material blackMaterial;
    public CameraAligner cameraAligner;

    private GameObject gridParent;

    public void GenerateGrid()
    {
        DeleteGrid();
        gridParent = new GameObject("GridParent");

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.parent = gridParent.transform;

                // Posizionamento con offset Z
                cube.transform.position = new Vector3(col * cubeSize, 0, row * cubeSize + offsetZ);
                cube.transform.localScale = new Vector3(cubeSize, scaleY, cubeSize);

                if ((row + col) % 2 == 0)
                {
                    // Default white color
                }
                else
                {
                    Renderer renderer = cube.GetComponent<Renderer>();
                    renderer.material = blackMaterial;
                }

                if (!singleCollider)
                {
                    cube.AddComponent<BoxCollider>();
                }
                else
                {
                    DestroyImmediate(cube.GetComponent<Collider>());
                }
            }
        }

        if (singleCollider)
        {
            AddUnifiedCollider();
        }

        if (cameraAligner != null)
        {
            cameraAligner.AlignCameraToGrid(rows, columns, cubeSize);
        }
    }

    private void AddUnifiedCollider()
    {
        GameObject gridCollider = new GameObject("GridCollider");
        gridCollider.transform.parent = gridParent.transform;

        BoxCollider collider = gridCollider.AddComponent<BoxCollider>();
        float gridWidth = columns * cubeSize;
        float gridHeight = rows * cubeSize;

        collider.size = new Vector3(gridWidth, scaleY, gridHeight);
        collider.center = new Vector3(gridWidth / 2f - cubeSize / 2f, 0, gridHeight / 2f - cubeSize / 2f + offsetZ);
    }

    public void DeleteGrid()
    {
        if (gridParent != null)
        {
            DestroyImmediate(gridParent);
        }
    }
}
