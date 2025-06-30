using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject grid;
    public int width;
    public int height;

    private Vector2[,] matrixTransforms;

    private void Awake()
    {
        if (grid == null)
        {
            Debug.LogError("Grid prefab is not assigned!");
            return;
        }

        // Instantiate one grid temporarily to get its size
        GameObject tempGrid = Instantiate(grid);
        Renderer gridRenderer = tempGrid.GetComponent<Renderer>();

        if (gridRenderer == null)
        {
            Debug.LogError("Grid prefab must have a Renderer component.");
            Destroy(tempGrid);
            return;
        }

        Vector3 gridSize = gridRenderer.bounds.size;
        Destroy(tempGrid);

        matrixTransforms = new Vector2[width, height];

        // Calculate total board size
        float boardWidth = gridSize.x * width;
        float boardHeight = gridSize.y * height;

        // Calculate starting point to center the grid
        float startX = -boardWidth / 2f + gridSize.x / 2f;
        float startY = -boardHeight / 2f + gridSize.y / 2f;

        // Instantiate grids side-by-side with no scaling
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(startX + x * gridSize.x, startY + y * gridSize.y);
                matrixTransforms[x, y] = pos;

                GameObject tile = Instantiate(grid, transform);
                tile.transform.localPosition = pos;
                tile.transform.localScale = Vector3.one;
            }
        }

        // Adjust camera position based on combined bounds of instantiated grids
        AdjustCameraZUsingBounds();
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

        // Add margin/padding to the bounds size (e.g., 5% padding)
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
