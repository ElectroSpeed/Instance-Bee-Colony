using UnityEngine;

public class Cell
{
    public Vector2Int position;
    public int gCost = int.MaxValue;
    public bool isWalkable;
    public Cell parent;
    public bool inClosedSet;

    public Cell(int x, int y, bool walkable)
    {
        position = new Vector2Int(x, y);
        isWalkable = walkable;
    }

    public void Reset()
    {
        gCost = int.MaxValue;
        parent = null;
        inClosedSet = false;
    }
}