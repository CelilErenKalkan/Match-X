using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject gridPrefab;
    public int width;
    public int height;

    private Grid[,] matrixGrids;
    private Coroutine checkCoroutine;

    private void Awake()
    {
        if (gridPrefab == null)
        {
            Debug.LogError("Grid prefab is not assigned!");
            return;
        }

        // Instantiate one grid temporarily to get its size
        GameObject tempGrid = Instantiate(gridPrefab);
        Renderer gridRenderer = tempGrid.GetComponent<Renderer>();

        if (gridRenderer == null)
        {
            Debug.LogError("Grid prefab must have a Renderer component.");
            Destroy(tempGrid);
            return;
        }

        Vector3 gridSize = gridRenderer.bounds.size;
        Destroy(tempGrid);

        matrixGrids = new Grid[width, height];

        float boardWidth = gridSize.x * width;
        float boardHeight = gridSize.y * height;

        float startX = -boardWidth / 2f + gridSize.x / 2f;
        float startY = -boardHeight / 2f + gridSize.y / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(startX + x * gridSize.x, startY + y * gridSize.y);
                GameObject tile = Instantiate(gridPrefab, transform);
                tile.transform.localPosition = pos;
                tile.transform.localScale = Vector3.one;

                Grid gridScript = tile.GetComponent<Grid>();
                if (gridScript == null)
                {
                    Debug.LogError("Grid prefab must have a Grid script attached.");
                    Destroy(tile);
                    continue;
                }

                matrixGrids[x, y] = gridScript;
            }
        }

        Grid.GridSelected += OnGridSelected;

        AdjustCameraZUsingBounds();
    }

    private void OnDestroy()
    {
        Grid.GridSelected -= OnGridSelected;
    }

    private void OnGridSelected(Grid selectedGrid)
    {
        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(CheckAfterDelay(selectedGrid));
    }

    private System.Collections.IEnumerator CheckAfterDelay(Grid selectedGrid)
    {
        yield return new WaitForSeconds(0.3f);  // delay to show selection before reset

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (matrixGrids[x, y] == selectedGrid)
                {
                    CheckConnectedSelectedGrids(x, y);
                    yield break;
                }
            }
        }
    }

    private void CheckConnectedSelectedGrids(int startX, int startY)
    {
        List<(int x, int y)> connected = GridClusterFinder.FindConnectedSelectedCluster(matrixGrids, startX, startY);

        if (connected.Count >= 3)
        {
            foreach (var (x, y) in connected)
            {
                matrixGrids[x, y].ResetGrid();
            }
        }
    }

    private void AdjustCameraZUsingBounds()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }
        if (cam.orthographic)
        {
            Debug.LogWarning("This method works for perspective cameras only.");
            return;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("No renderers found in grid children.");
            return;
        }

        Bounds combinedBounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 boundsCenter = combinedBounds.center;
        Vector3 boundsSize = combinedBounds.size;

        float marginFactor = 1.02f;
        boundsSize *= marginFactor;

        Vector3 camPos = cam.transform.position;
        camPos.x = boundsCenter.x;
        camPos.y = boundsCenter.y;

        float fov = cam.fieldOfView;
        float aspect = cam.aspect;

        float halfVerticalFOV = Mathf.Deg2Rad * fov * 0.5f;
        float halfHorizontalFOV = Mathf.Atan(Mathf.Tan(halfVerticalFOV) * aspect);

        float distanceVertical = (boundsSize.y / 2f) / Mathf.Tan(halfVerticalFOV);
        float distanceHorizontal = (boundsSize.x / 2f) / Mathf.Tan(halfHorizontalFOV);

        float requiredDistance = Mathf.Max(distanceVertical, distanceHorizontal);

        camPos.z = boundsCenter.z - requiredDistance;

        cam.transform.position = camPos;
    }
}
