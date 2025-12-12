using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int _position;
    public float _height;
    public float _pathPointHeight;
    public bool _isWalkable=true;

    [HideInInspector] public Transform _pathPoint;
    [HideInInspector] public int _gCost = int.MaxValue;
    [HideInInspector] public Cell _parent;
    [HideInInspector] public bool _inClosedSet = false;

    public void Reset()
    {
        _gCost = int.MaxValue;
        _parent = null;
        _inClosedSet = false;
    }

    private void Awake()
    {
        if (transform.childCount > 1)
            _pathPoint = transform.GetChild(1);
    }
}