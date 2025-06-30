using UnityEngine;

public static class TileSpawner
{
    public static Tile[,] CreateTileMatrix(GameObject prefab, Transform parent, int width, int height, out Vector3 cellSize)
    {
        GameObject temp = Object.Instantiate(prefab);
        Renderer r = temp.GetComponent<Renderer>();
        cellSize = r.bounds.size;
        Object.Destroy(temp);

        Tile[,] matrix = new Tile[width, height];

        float startX = -cellSize.x * width / 2f + cellSize.x / 2f;
        float startY = -cellSize.y * height / 2f + cellSize.y / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(startX + x * cellSize.x, startY + y * cellSize.y);
                GameObject tileObj = Object.Instantiate(prefab, parent);
                tileObj.transform.localPosition = pos;
                tileObj.transform.localScale = Vector3.one;

                Tile tile = tileObj.GetComponent<Tile>();
                matrix[x, y] = tile;
            }
        }

        return matrix;
    }
}