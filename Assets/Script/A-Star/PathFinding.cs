using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
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

    private Coroutine _pathfindingRoutine;

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

    public IEnumerator FindPath(Vector3 startWorld, Vector3 endWorld, Action<List<Cell>> callback)
    {
        if (MapGenerator.Instance == null)
        {
            Debug.LogError("MapGenerator.instance is null");
            callback?.Invoke(null);
            yield break;
        }

        Vector2Int startGrid = MapGenerator.Instance.WorldToGrid(startWorld);
        Vector2Int endGrid = MapGenerator.Instance.WorldToGrid(endWorld);

        if (!MapGenerator.Instance._graph.TryGetValue(startGrid, out Cell start))
        {
            Debug.LogError($"Start position {startGrid} is outside of map.");
            callback?.Invoke(null);
            yield break;
        }

        if (!MapGenerator.Instance._graph.TryGetValue(endGrid, out Cell end))
        {
            Debug.LogWarning($"End position {endGrid} is outside of map.");
            callback?.Invoke(null);
            yield break;
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
                callback?.Invoke(_path);
                yield break;
            }

            current._inClosedSet = true;

            foreach (Cell neighbor in GetNeighbors(current))
            {
                if (neighbor._inClosedSet) continue;

                if (!neighbor._isWalkable) continue;

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

        callback?.Invoke(null);
        yield break;
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

    public IEnumerator FindPathPositions(Vector3 startWorld, Vector3 endWorld, Action<List<Vector3>> callback)
    {
        int smoothResolution = 50;
        List<Cell> cellPath = new();

        yield return FindPath(startWorld, endWorld, result =>
        {
            cellPath = result;
        });

        if (cellPath.Count <= 0) yield break;

        List<Vector3> positions = new List<Vector3>();
        foreach (Cell c in cellPath)
        {
            if (c._pathPoint != null)
                positions.Add(c._pathPoint.position);
            else
                positions.Add(MapGenerator.Instance.GridToWorld(c._position));
        }
        
        if (positions.Count > 1)
        {
            positions = BSplineSmooth(positions, smoothResolution);
        }


        callback?.Invoke(positions);
        yield break;
    }

    #region B-Spline
    private List<Vector3> BSplineSmooth(List<Vector3> points, int resolution)
    {
        if (points.Count < 2) return points;

        List<Vector3> smoothPoints = new List<Vector3>();
        smoothPoints.Add(points[0]);
        smoothPoints.Add(points[0]);
        smoothPoints.AddRange(points);
        smoothPoints.Add(points[points.Count - 1]);
        smoothPoints.Add(points[points.Count - 1]);

        return BSplineRaw(smoothPoints, resolution);
    }

    private List<Vector3> BSplineRaw(List<Vector3> controlPoints, int resolution)
    {
        List<Vector3> smoothedPoints = new List<Vector3>();
        
        for (int segmentIndex = 0; segmentIndex < controlPoints.Count - 3; segmentIndex++)
        {
            for (int stepIndex = 0; stepIndex <= resolution; stepIndex++)
            {
                float normalizedTime = stepIndex / (float)resolution;

                Vector3 pointOnSpline = EvaluateCubicBSpline(normalizedTime,
                    controlPoints[segmentIndex], 
                    controlPoints[segmentIndex + 1], 
                    controlPoints[segmentIndex + 2], 
                    controlPoints[segmentIndex + 3]
                );
                
                smoothedPoints.Add(pointOnSpline);
            }
        }

        return smoothedPoints;
    }

    private Vector3 EvaluateCubicBSpline(float normalizedTime, Vector3 previousPoint, Vector3 startPoint, Vector3 endPoint, Vector3 nextPoint)
    {
        float normalizedTimeSquared = normalizedTime * normalizedTime;
        float normalizedTimeCubed   = normalizedTimeSquared * normalizedTime;

        float weightPrevious = (-normalizedTimeCubed + 3f * normalizedTimeSquared - 3f * normalizedTime + 1f) / 6f;
        float weightStart = (3f * normalizedTimeCubed - 6f * normalizedTimeSquared + 4f) / 6f;
        float weightEnd = (-3f * normalizedTimeCubed + 3f * normalizedTimeSquared + 3f * normalizedTime + 1f) / 6f;
        float weightNext = normalizedTimeCubed / 6f;

        return weightPrevious * previousPoint + weightStart * startPoint + weightEnd * endPoint + weightNext * nextPoint;
    }
    #endregion
}
