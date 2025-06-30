using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width;
    public int height;

    private Tile[,] tileMatrix;
    private Coroutine checkCoroutine;

    private void Awake()
    {
        tileMatrix = TileSpawner.CreateTileMatrix(tilePrefab, transform, width, height, out _);
        Actions.TileSelected += OnTileSelected;
        CameraAdjuster.FitCameraToRenderers(Camera.main, GetComponentsInChildren<Renderer>());
    }

    private void OnDestroy()
    {
        Actions.TileSelected -= OnTileSelected;
    }

    private void OnTileSelected(Tile tile)
    {
        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(DelayedCheck(tile));
    }

    private System.Collections.IEnumerator DelayedCheck(Tile tile)
    {
        yield return new WaitForSeconds(0.3f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tileMatrix[x, y] == tile)
                {
                    List<(int x, int y)> group = TileClusterFinder.FindConnectedSelectedCluster(tileMatrix, x, y);
                    if (group.Count >= 3)
                    {
                        foreach (var (gx, gy) in group)
                            tileMatrix[gx, gy].ResetTile();
                    }
                    yield break;
                }
            }
        }
    }
}