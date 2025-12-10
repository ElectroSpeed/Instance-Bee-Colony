using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private List<Cell> _cellNeighbors = new List<Cell>();
    private List<Cell> _usedCells = new List<Cell>();
    private List<Cell> _path = new List<Cell>();

    private static readonly Vector2Int[] _evenRow =
    {
        new Vector2Int(-1, 0), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(0, 1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1),
    };

    private static readonly Vector2Int[] _oddRow =
    {
        new Vector2Int(-1, 0), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(0, 1),
        new Vector2Int(1, -1), new Vector2Int(1, 1),
    };

    private List<Cell> GetNeighbors(Cell cell)
    {
        _cellNeighbors.Clear();

        bool isEven = (cell._position.y % 2) == 0;
        Vector2Int[] dirs = isEven ? _evenRow : _oddRow;

        foreach (var d in dirs)
        {
            Vector2Int nPos = cell._position + d;
            if (MapGenerator.Instance._graph.TryGetValue(nPos, out Cell neighbor))
            {
                _cellNeighbors.Add(neighbor);
            }
        }

        return _cellNeighbors;
    }

    private int Heuristic(Cell a, Cell b)
    {
        int dx = Mathf.Abs(a._position.x - b._position.x);
        int dy = Mathf.Abs(a._position.y - b._position.y);
        return dx + dy - Mathf.Min(dx, dy);
    }

    public List<Cell> FindPath(Vector3 startWorld, Vector3 endWorld)
    {
        if (MapGenerator.Instance == null)
        {
            Debug.LogError("MapGenerator.instance is null");
            return null;
        }

        Vector2Int startGrid = MapGenerator.Instance.WorldToGrid(startWorld);
        Vector2Int endGrid = MapGenerator.Instance.WorldToGrid(endWorld);

        if (!MapGenerator.Instance._graph.TryGetValue(startGrid, out Cell start))
        {
            Debug.LogError($"Start position {startGrid} is outside of map.");
            return null;
        }

        if (!MapGenerator.Instance._graph.TryGetValue(endGrid, out Cell end))
        {
            Debug.LogError($"End position {endGrid} is outside of map.");
            return null;
        }

        ResetUsedCells();
        _path.Clear();

        PriorityQueue<Cell> openSet = new PriorityQueue<Cell>();

        start._gCost = 0;
        start._parent = null;

        openSet.Enqueue(start, Heuristic(start, end));
        AddToUsed(start);

        while (openSet.Count > 0)
        {
            Cell current = openSet.Dequeue();

            if (current == end)
            {
                _path = BuildPath(end);
                ResetUsedCells();
                return _path;
            }

            current.inClosedSet = true;
            
            foreach (Cell neighbor in GetNeighbors(current))
            {
                if (neighbor.inClosedSet) continue;

                float heightDiff = Mathf.Max(0, neighbor._height - current._height);
                int heightCost = Mathf.RoundToInt(heightDiff * 10);
                int pathCost = current._gCost + 1 + heightCost;

                if (pathCost < neighbor._gCost)
                {
                    neighbor._gCost = pathCost;
                    neighbor._parent = current;

                    int f = neighbor._gCost + Heuristic(neighbor, end);

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, f);

                    AddToUsed(neighbor);
                }
            }
        }

        Debug.LogWarning("No path found.");
        ResetUsedCells();
        return null;
    }

    private List<Cell> BuildPath(Cell end)
    {
        List<Cell> result = new List<Cell>();
        Cell current = end;

        while (current != null)
        {
            result.Add(current);
            current = current._parent;
        }

        result.Reverse();
        return result;
    }

    private void AddToUsed(Cell c)
    {
        if (!_usedCells.Contains(c))
            _usedCells.Add(c);
    }

    private void ResetUsedCells()
    {
        foreach (Cell c in _usedCells)
            c.Reset();

        _usedCells.Clear();
    }
    
    public List<Vector3> FindPathPositions(Vector3 startWorld, Vector3 endWorld)
    {
        List<Cell> cellPath = FindPath(startWorld, endWorld);
        if (cellPath == null)
            return null;

        List<Vector3> positions = new List<Vector3>();

        foreach (Cell c in cellPath)
        {
            if (c._pathPoint != null)
                positions.Add(c._pathPoint.position);
            else
                positions.Add(MapGenerator.Instance.GridToWorld(c._position));
        }

        return positions;
    }
}