using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    [Header("Map settings")]
    [SerializeField] private Cell _tilePrefab;
    [SerializeField] private int _gridLength = 10;
    [SerializeField] private int _gridWidth = 10;
    
    public Dictionary<Vector2Int, Cell> _graph = new Dictionary<Vector2Int, Cell>();
    
    public Vector3 _cellSizes = Vector3.zero;

    private void Awake()
    {
        if (_tilePrefab == null)
        {
            return;
        }
        
        Cell temp = Instantiate(_tilePrefab, Vector3.zero, _tilePrefab.transform.rotation, transform);
        
        Renderer rend = temp.GetComponentInChildren<Renderer>();
        if (rend == null)
        {
            Debug.LogError("MapGenerator: Aucun Renderer trouvé sur le prefab de tuile.");
            Destroy(temp.gameObject);
            return;
        }
        
        Vector3 worldSize = rend.bounds.size;
        
        _cellSizes = worldSize;
        
        Destroy(temp.gameObject);
    }

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        foreach (var cell in _graph.Values)
            if (cell != null) Destroy(cell.gameObject);

        _graph.Clear();
        
        float cellWidth = _cellSizes.x;
        float cellDepth = _cellSizes.z;
        
        // For hex flat-top, vertical overlap ~ 25% => vertical step = depth * 0.75
        float xStep = cellWidth;
        float yStep = cellDepth * 0.75f;

        for (int y = 0; y < _gridWidth; y++)
        {
            for (int x = 0; x < _gridLength; x++)
            {
                float offset = (y % 2 == 1) ? cellWidth / 2f : 0f;

                Vector3 pos = new Vector3(
                    x * xStep + offset,
                    0f,
                    y * yStep
                );
                
                Cell tile = Instantiate(_tilePrefab, pos, _tilePrefab.transform.rotation, transform);
                _graph.Add(new Vector2Int(x, y), tile);
            }
        }
    }

    public Vector2Int WorldToGrid(Vector3 world)
    {
        float cellWidth = _cellSizes.x;
        float cellDepth = _cellSizes.z;
        float yStep = cellDepth * 0.75f;
        
        int gy = Mathf.RoundToInt(world.z / yStep);

        float offset = (gy % 2 == 1) ? cellWidth / 2f : 0f;
        int gx = Mathf.RoundToInt((world.x - offset) / cellWidth);

        return new Vector2Int(gx, gy);
    }

    public Vector3 GridToWorld(Vector2Int grid)
    {
        float cellWidth = _cellSizes.x;
        float cellDepth = _cellSizes.z;
        float offset = (grid.y % 2 == 1) ? cellWidth / 2f : 0f;

        return new Vector3(
            grid.x * cellWidth + offset,
            0f,
            grid.y * cellDepth * 0.75f
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (_graph == null) return;

        foreach (var kv in _graph)
        {
            Vector3 w = GridToWorld(kv.Key) + Vector3.up * 1f;
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(w, 0.15f);
        }
    }
}
