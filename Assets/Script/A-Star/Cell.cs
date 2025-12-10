using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int _position;
    public float _height;

    [HideInInspector] public Transform _pathPoint;
    public int _gCost = int.MaxValue;
    public Cell _parent;
    public bool inClosedSet = false;

    public void Reset()
    {
        _gCost = int.MaxValue;
        _parent = null;
        inClosedSet = false;
    }

    private void Awake()
    {
        if (transform.childCount > 1)
            _pathPoint = transform.GetChild(1);
    }
}