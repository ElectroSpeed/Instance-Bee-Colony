using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int position;
    public int gCost = int.MaxValue;
    public Cell parent;
    public bool inClosedSet;

    public void Reset()
    {
        gCost = int.MaxValue;
        parent = null;
        inClosedSet = false;
    }
}