using System.Collections.Generic;

public static class TileClusterFinder
{
    public static List<(int x, int y)> FindConnectedSelectedCluster(Tile[,] matrix, int startX, int startY)
    {
        int width = matrix.GetLength(0);
        int height = matrix.GetLength(1);

        List<(int x, int y)> connected = new List<(int, int)>();
        Queue<(int x, int y)> toCheck = new Queue<(int, int)>();
        bool[,] visited = new bool[width, height];

        toCheck.Enqueue((startX, startY));
        visited[startX, startY] = true;

        while (toCheck.Count > 0)
        {
            var (x, y) = toCheck.Dequeue();
            connected.Add((x, y));

            TryAdd(x + 1, y);
            TryAdd(x - 1, y);
            TryAdd(x, y + 1);
            TryAdd(x, y - 1);
        }

        return connected;

        void TryAdd(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return;
            if (visited[x, y]) return;
            if (!matrix[x, y].IsSelected()) return;

            visited[x, y] = true;
            toCheck.Enqueue((x, y));
        }
    }
}