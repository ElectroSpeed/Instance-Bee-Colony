using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private List<Cell> tempNeighbors = new List<Cell>();
    private List<Cell> usedCells = new List<Cell>();
    private List<Cell> path = new List<Cell>();

    private static readonly Vector2Int[] evenRow =
    {
        new Vector2Int(-1, 0), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(0, 1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1),
    };

    private static readonly Vector2Int[] oddRow =
    {
        new Vector2Int(-1, 0), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(0, 1),
        new Vector2Int(1, -1), new Vector2Int(1, 1),
    };

    private List<Cell> GetNeighbors(Cell cell)
    {
        tempNeighbors.Clear();

        bool isEven = (cell.position.y % 2) == 0;
        Vector2Int[] dirs = isEven ? evenRow : oddRow;

        foreach (var d in dirs)
        {
            Vector2Int nPos = cell.position + d;
            if (MapGenerator.instance.graph.TryGetValue(nPos, out Cell n) && n.isWalkable)
            {
                tempNeighbors.Add(n);
            }
        }

        return tempNeighbors;
    }

    private int Heuristic(Cell a, Cell b)
    {
        int dx = Mathf.Abs(a.position.x - b.position.x);
        int dy = Mathf.Abs(a.position.y - b.position.y);
        return Mathf.Max(dx, dy);
    }

    public List<Cell> FindPath(Vector3 startWorld, Vector3 endWorld)
    {
        if (MapGenerator.instance == null)
        {
            Debug.LogError("MapGenerator.instance is null");
            return null;
        }

        Vector2Int startGrid = MapGenerator.instance.WorldToGrid(startWorld);
        Vector2Int endGrid = MapGenerator.instance.WorldToGrid(endWorld);

        Debug.Log($"Start GRID: {startGrid} | End GRID: {endGrid}");

        if (!MapGenerator.instance.graph.TryGetValue(startGrid, out Cell start))
        {
            Debug.LogError("START is outside grid: " + startGrid);
            return null;
        }

        if (!MapGenerator.instance.graph.TryGetValue(endGrid, out Cell end))
        {
            Debug.LogError("END is outside grid: " + endGrid);
            return null;
        }

        ResetUsedCells();
        path.Clear();

        PriorityQueue<Cell> open = new PriorityQueue<Cell>();

        start.gCost = 0;
        start.parent = null;

        open.Enqueue(start, Heuristic(start, end));
        AddToUsed(start);

        while (open.Count > 0)
        {
            Cell current = open.Dequeue();

            if (current == end)
            {
                path = BuildPath(end);
                Debug.Log("PATH FOUND: " + path.Count);
                ResetUsedCells();
                return path;
            }

            current.inClosedSet = true;

            foreach (Cell neighbor in GetNeighbors(current))
            {
                if (neighbor.inClosedSet) continue;

                int tentativeG = current.gCost + 1;

                if (tentativeG < neighbor.gCost)
                {
                    neighbor.gCost = tentativeG;
                    neighbor.parent = current;

                    int f = neighbor.gCost + Heuristic(neighbor, end);

                    if (!open.Contains(neighbor))
                        open.Enqueue(neighbor, f);

                    AddToUsed(neighbor);
                }
            }
        }

        Debug.LogWarning("NO PATH FOUND");
        ResetUsedCells();
        return null;
    }

    private List<Cell> BuildPath(Cell end)
    {
        List<Cell> result = new List<Cell>();
        Cell c = end;

        while (c != null)
        {
            result.Add(c);
            c = c.parent;
        }

        result.Reverse();
        return result;
    }

    private void AddToUsed(Cell c)
    {
        if (!usedCells.Contains(c))
            usedCells.Add(c);
    }

    private void ResetUsedCells()
    {
        foreach (Cell c in usedCells)
            c.Reset();

        usedCells.Clear();
    }
}