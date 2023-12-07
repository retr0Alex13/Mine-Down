using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DepthFirstSearch
{
    private bool[,] visited;
    private List<Vector2Int> path;

    public List<Vector2Int> FindPath(int rows, int cubesPerRow)
    {
        visited = new bool[rows, cubesPerRow];
        path = new List<Vector2Int>();

        // Start DFS from a random position in the top row
        int startingCubeIndex = Random.Range(0, cubesPerRow);
        DFS(0, startingCubeIndex, rows, cubesPerRow);

        return path;
    }

    private void DFS(int row, int cubeIndex, int rows, int cubesPerRow)
    {
        visited[row, cubeIndex] = true;
        path.Add(new Vector2Int(cubeIndex, row));

        // Check neighbors (down, left, and right)
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(cubeIndex, row + 1),
            new Vector2Int(cubeIndex - 1, row),
            new Vector2Int(cubeIndex + 1, row)
        };

        // Shuffle the neighbors to introduce randomness
        neighbors = neighbors.OrderBy(x => Random.value).ToList();

        foreach (var neighbor in neighbors)
        {
            int neighborCubeIndex = neighbor.x;
            int neighborRow = neighbor.y;

            if (neighborRow < rows && neighborCubeIndex >= 0 && neighborCubeIndex < cubesPerRow &&
                !visited[neighborRow, neighborCubeIndex])
            {
                DFS(neighborRow, neighborCubeIndex, rows, cubesPerRow);
                break;  // Ensure only one path is created
            }
        }
    }
}
