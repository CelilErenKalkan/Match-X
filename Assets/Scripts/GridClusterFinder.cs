using System.Collections.Generic;

public static class GridClusterFinder
{
    /// <summary>
    /// Returns the list of connected selected grids starting from (startX, startY) within the matrix.
    /// Uses 4-direction adjacency (up, down, left, right).
    /// </summary>
    public static List<(int x, int y)> FindConnectedSelectedCluster(Grid[,] matrix, int startX, int startY)
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

            TryAddNeighbor(matrix, x + 1, y, visited, toCheck, width, height);
            TryAddNeighbor(matrix, x - 1, y, visited, toCheck, width, height);
            TryAddNeighbor(matrix, x, y + 1, visited, toCheck, width, height);
            TryAddNeighbor(matrix, x, y - 1, visited, toCheck, width, height);
        }

        return connected;
    }

    private static void TryAddNeighbor(Grid[,] matrix, int x, int y, bool[,] visited, Queue<(int x, int y)> queue, int width, int height)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return;
        if (visited[x, y]) return;
        if (!matrix[x, y].IsSelected()) return;

        visited[x, y] = true;
        queue.Enqueue((x, y));
    }
}
