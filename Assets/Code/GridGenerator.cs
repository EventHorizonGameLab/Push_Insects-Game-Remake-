using UnityEngine;

[ExecuteInEditMode]
public class GridGenerator : MonoBehaviour
{
    public int rows = 5; // Numero di righe
    public int columns = 5; // Numero di colonne
    public float cubeSize = 1f; // Dimensione dei cubi
    public float scaleY = 1f; // Scala in Y
    public bool singleCollider = false; // Flag per un unico collider
    private GameObject gridParent; // Per organizzare i cubetti
    private GameObject gridCollider; // Collider unico

    // Metodo per generare la griglia
    public void GenerateGrid()
    {
        DeleteGrid(); // Rimuovi qualsiasi griglia esistente
        gridParent = new GameObject("GridParent");

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.parent = gridParent.transform;
                cube.transform.position = new Vector3(col * cubeSize, 0, row * cubeSize);
                cube.transform.localScale = new Vector3(cubeSize, scaleY, cubeSize);

                if (!singleCollider)
                {
                    // Mantieni il collider individuale
                    cube.AddComponent<BoxCollider>();
                }
                else
                {
                    // Rimuovi il collider individuale
                    DestroyImmediate(cube.GetComponent<Collider>());
                }
            }
        }

        if (singleCollider)
        {
            AddUnifiedCollider();
        }
    }

    // Metodo per aggiungere un collider unico
    private void AddUnifiedCollider()
    {
        gridCollider = new GameObject("GridCollider");
        gridCollider.transform.parent = gridParent.transform;

        BoxCollider collider = gridCollider.AddComponent<BoxCollider>();
        float gridWidth = columns * cubeSize;
        float gridHeight = rows * cubeSize;

        collider.size = new Vector3(gridWidth, scaleY, gridHeight);
        collider.center = new Vector3(gridWidth / 2f - cubeSize / 2f, 0, gridHeight / 2f - cubeSize / 2f);
    }

    // Metodo per eliminare la griglia
    public void DeleteGrid()
    {
        if (gridParent != null)
        {
            DestroyImmediate(gridParent);
        }
    }
}