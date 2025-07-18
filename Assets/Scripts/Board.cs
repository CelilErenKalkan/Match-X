using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width;
    public int height;
    private const string WidthKey = "Width";
    private const string HeightKey = "Height";

    private Tile[,] tileMatrix;
    private Coroutine checkCoroutine;

    private void Awake()
    {
        LoadBoard();
        GenerateBoard(width, height);
        Actions.TileSelected += OnTileSelected;
    }

    private void OnDestroy()
    {
        Actions.TileSelected -= OnTileSelected;
    }

    public void GenerateBoard(int newWidth, int newHeight)
    {
        width = newWidth;
        height = newHeight;

        // Clear previous tiles
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Create new tile matrix
        tileMatrix = TileSpawner.CreateTileMatrix(tilePrefab, transform, width, height, out _);
        StartCoroutine(FitCameraNextFrame());
    }

    private void OnTileSelected(Tile tile)
    {
        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(DelayedCheck(tile));
    }

    private IEnumerator FitCameraNextFrame()
    {
        yield return null; // Wait one frame to ensure old tiles are destroyed
        CameraAdjuster.FitCameraToRenderers(Camera.main, GetComponentsInChildren<Renderer>());
        SaveBoard();
    }

    private IEnumerator DelayedCheck(Tile tile)
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
                        {
                            tileMatrix[gx, gy].ResetTile();
                        }
                        Actions.Match?.Invoke();
                    }
                    yield break;
                }
            }
        }
    }
    
    private void SaveBoard()
    {
        PlayerPrefs.SetInt(WidthKey, width);
        PlayerPrefs.SetInt(HeightKey, height);
        PlayerPrefs.Save();
    }

    private void LoadBoard()
    {
        width = PlayerPrefs.GetInt(WidthKey, 4); // 4 is the default value if key doesn't exist
        height = PlayerPrefs.GetInt(HeightKey, 4); // 4 is the default value if key doesn't exist
    }
}
