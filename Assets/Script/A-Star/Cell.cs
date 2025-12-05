using UnityEngine;

public class Cell
{
    public Vector2Int position;
    public int gCost = int.MaxValue;
    public float cellWeight;
    public Cell parent;
    public bool inClosedSet;

    public Cell(int x, int y, float weight)
    {
        position = new Vector2Int(x, y);
        cellWeight = weight;
    }

    public void Reset()
    {
        gCost = int.MaxValue;
        parent = null;
        inClosedSet = false;
    }
}