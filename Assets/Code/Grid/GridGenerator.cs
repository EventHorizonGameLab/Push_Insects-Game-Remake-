using UnityEngine;

[ExecuteInEditMode]
public class GridGenerator : MonoBehaviour
{
    public enum GridSize
    {
        Grid6x6,
        Grid7x7,
        Grid6x8
    }

    public GridSize gridSize = GridSize.Grid6x6;
    public float cubeSize = 1f;
    public float scaleY = 1f;
    public float offsetZ = 0f; // Offset Z per la griglia
    public bool singleCollider = false;

    public Material blackMaterial;
    public CameraAligner cameraAligner;

    private GameObject gridParent;

    private int rows;
    private int columns;

    public void GenerateGrid()
    {
        SetGridDimensions();
        DeleteGrid();
        gridParent = new GameObject("GridParent");

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.parent = gridParent.transform;

                // Posizionamento
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

        AddWallsWithExit();

        if (cameraAligner != null)
        {
            cameraAligner.AlignCameraToGrid(rows, columns, cubeSize);
        }
    }

    private void SetGridDimensions()
    {
        switch (gridSize)
        {
            case GridSize.Grid6x6:
                rows = 6;
                columns = 6;
                break;
            case GridSize.Grid7x7:
                rows = 7;
                columns = 7;
                break;
            case GridSize.Grid6x8:
                rows = 6;
                columns = 8;
                break;
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

    private void AddWallsWithExit()
    {
        float wallHeight = 1f;
        GameObject wallsParent = new GameObject("Walls");
        wallsParent.transform.parent = gridParent.transform;

        float gridWidth = columns * cubeSize;
        float gridHeight = rows * cubeSize;

        // La quarta unità dal basso corrisponde a row index = 3
        int exitRow = 3;

        // Posizione dell'uscita in unità spaziali
        float exitPositionZ = (exitRow + 1) * cubeSize + offsetZ;

        // Muro destro superiore
        float upperWallHeight = exitRow * cubeSize;
        CreateWall(new Vector3(gridWidth, wallHeight / 2f, upperWallHeight / 2f - cubeSize / 2f + offsetZ),
                   new Vector3(cubeSize, wallHeight, upperWallHeight), wallsParent);

        // Muro destro inferiore
        float lowerWallHeight = gridHeight - (exitRow + 1) * cubeSize;
        CreateWall(new Vector3(gridWidth, wallHeight / 2f, gridHeight - lowerWallHeight / 2f - cubeSize / 2f + offsetZ),
                   new Vector3(cubeSize, wallHeight, lowerWallHeight), wallsParent);

        // Aggiungere collider nello spazio vuoto (uscita)
        GameObject exitCollider = new GameObject("ExitCollider");
        exitCollider.transform.parent = wallsParent.transform;

        BoxCollider exitBoxCollider = exitCollider.AddComponent<BoxCollider>();
        exitBoxCollider.size = new Vector3(1f, wallHeight, cubeSize);
        exitCollider.transform.position = new Vector3(gridWidth, wallHeight / 2f, exitPositionZ - cubeSize);

        // Impostare il layer al numero 7
        exitCollider.layer = 7;

        // Altri muri (alto, sinistra, basso)
        CreateWall(new Vector3(gridWidth / 2f - cubeSize / 2f, wallHeight / 2f, -cubeSize + offsetZ),
                   new Vector3(gridWidth, wallHeight, cubeSize), wallsParent); // Muro superiore
        CreateWall(new Vector3(gridWidth / 2f - cubeSize / 2f, wallHeight / 2f, gridHeight + offsetZ),
                   new Vector3(gridWidth, wallHeight, cubeSize), wallsParent); // Muro inferiore
        CreateWall(new Vector3(-cubeSize, wallHeight / 2f, gridHeight / 2f - cubeSize / 2f + offsetZ),
                   new Vector3(cubeSize, wallHeight, gridHeight), wallsParent); // Muro sinistro
    }




    private void CreateWall(Vector3 position, Vector3 size, GameObject parent)
    {
        GameObject wall = new GameObject("Wall");
        wall.transform.parent = parent.transform;

        BoxCollider collider = wall.AddComponent<BoxCollider>();
        collider.size = size;
        wall.transform.position = position;
    }

    public void DeleteGrid()
    {
        if (gridParent != null)
        {
            DestroyImmediate(gridParent);
        }
    }
}
