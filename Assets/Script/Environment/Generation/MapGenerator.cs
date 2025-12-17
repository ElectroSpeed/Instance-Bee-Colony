using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [Header("Map settings")]
    [SerializeField] private Cell _tilePrefab;
    [SerializeField] private int _gridLength = 10;
    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _chunkCountPerLine = 10;

    [Header("Perlin Noise Settings")]
    public int _seed = 12345;
    public float _scale = 20f;
    public int _octaves = 4;
    [Range(0f, 1f)] public float _persistance = 0.5f;
    public float _lacunarity = 2f;
    public Vector2 _noiseOffset = Vector2.zero;
    public Noise.NormalizeMode _normalizeMode = Noise.NormalizeMode.Local;

    [Header("Terrain Settings")]
    public AnimationCurve _mapCurve;
    public float _heightMultiplier = 3f;
    public Gradient _colorGradient;

    public Dictionary<Vector2Int, Cell> _graph = new Dictionary<Vector2Int, Cell>();
    public Vector3 _cellSizes = Vector3.zero;

    [SerializeField] private List<Mesh> _obstacles;
    
    [Header("Beehive Settings")]
    [SerializeField] private GameObject _beehivePrefab;
    [SerializeField] private int _marginMapBorder;

    [Header("Obstacles Settings")]
    [Range(0, 100)]
    [SerializeField] private float _obstacleSpawnChance;
    [SerializeField] private GameObject _environmentObstaclePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_tilePrefab == null) return;

        Cell testCell = Instantiate(_tilePrefab, Vector3.zero, _tilePrefab.transform.rotation, transform);
        Renderer r = testCell.GetComponentInChildren<Renderer>();
        if (r == null)
        {
            Debug.LogError("MapGenerator: Aucun Renderer trouvé sur le prefab de tuile.");
            Destroy(testCell.gameObject);
            return;
        }

        _cellSizes = r.bounds.size;
        Destroy(testCell.gameObject);
    }

    private void Start()
    {
        GenerateMap();
    }

    private void ClearOlderGraph()
    {
        foreach (var cell in _graph.Values)
            if (cell != null) Destroy(cell.gameObject);

        _graph.Clear();
    }

    public void GenerateMap()
    {
        ClearOlderGraph();

        float[,] noiseMap = Noise.GenerateNoiseMap(
            _gridLength, _gridWidth, _seed, _scale,
            _octaves, _persistance, _lacunarity,
            _noiseOffset, _normalizeMode
        );

        float cellLength = _cellSizes.x;
        float cellWidth = _cellSizes.z;

        float xStep = cellLength;
        float yStep = cellWidth * 0.75f;

        for (int y = 0; y < _gridWidth; y++)
        {
            for (int x = 0; x < _gridLength; x++)
            {
                float offset = (y % 2 == 1) ? cellLength / 2f : 0f;

                Vector3 cellPositon = new Vector3(x * xStep + offset, 0f, y * yStep);
                
                Cell mapCell = Instantiate(_tilePrefab, cellPositon, _tilePrefab.transform.rotation, transform.GetChild(0));

                mapCell._position = new Vector2Int(x, y);

                noiseMap[x, y] = _mapCurve.Evaluate(noiseMap[x, y]);
                float noiseValue = noiseMap[x, y];
                
                _graph.Add(new Vector2Int(x, y), mapCell);
                
                GenerateMapCellProperty(mapCell, noiseValue);
                GenerateMapCellRenderer(mapCell, noiseValue);
                    
                if (Random.Range(0f, 100f) <= _obstacleSpawnChance)
                {
                    mapCell._isWalkable = false;
                    GenerateObstacles(mapCell, _obstacles[Random.Range(0, _obstacles.Count)]);
                }
            }
        }

        GenerateBeehive();
    }


    #region Generate Map

    private void GenerateMapCellProperty(Cell mapCell, float noiseValue)
    {
        Transform mapCellTransform = mapCell.transform.GetChild(0).GetChild(0);

        if (mapCellTransform != null)
        {
            float height = Mathf.Max(0.1f, noiseValue * _heightMultiplier);
            mapCell._height = height;

            Vector3 meshScale = mapCellTransform.localScale;
            meshScale.z = height / 2f;
            mapCellTransform.localScale = meshScale;

            Vector3 meshPos = mapCellTransform.localPosition;
            meshPos.y = height / 4f;
            mapCellTransform.localPosition = meshPos;

            Transform pathPoint = mapCell.transform.GetChild(1);
            if (pathPoint != null)
            {
                Vector3 pathPos = pathPoint.position;
                pathPos.y = (height / 2f) + mapCell._pathPointHeight;
                pathPoint.position = pathPos;
            }
        }
    }

    private void GenerateMapCellRenderer(Cell mapCell, float noiseValue)
    {
        UnityEngine.Renderer mapCellRenderer = mapCell.GetComponentInChildren<UnityEngine.Renderer>();
        if (mapCellRenderer != null)
            mapCellRenderer.material.color = _colorGradient.Evaluate(noiseValue);
    }
    
    private void GenerateObstacles(Cell mapCell, Mesh meshObstacle)
    {
        Vector3 obstaclePosition = mapCell._pathPoint.position;
        
        GameObject obstacle = Instantiate(_environmentObstaclePrefab, obstaclePosition, Quaternion.identity, transform.GetChild(1));
        obstacle.transform.position = new Vector3(obstacle.transform.position.x, obstacle.transform.position.y - mapCell._pathPointHeight, obstacle.transform.position.z);
        obstacle.GetComponentInChildren<MeshFilter>().mesh = meshObstacle;
    }
    
    private void GenerateBeehive()
    {
        List<Cell> validCells = new List<Cell>();

        foreach (var keyValuePair in _graph)
        {
            Vector2Int position = keyValuePair.Key;
            Cell cell = keyValuePair.Value;
            
            if (!cell._isWalkable) continue;
            
            if (position.x < _marginMapBorder ||
                position.y < _marginMapBorder ||
                position.x >= _gridLength - _marginMapBorder ||
                position.y >= _gridWidth  - _marginMapBorder)
                continue;

            validCells.Add(cell);
        }
        
        if (validCells.Count == 0)
        {
            return;
        }
        
        Cell beehiveCell = validCells[Random.Range(0, validCells.Count)];
        
        beehiveCell._isWalkable = false;
        Vector3 beehivePosition = beehiveCell._pathPoint.position;

        GameObject beehive = Instantiate(_beehivePrefab, beehivePosition, Quaternion.identity, transform.GetChild(2));
        beehive.transform.position = new Vector3(beehive.transform.position.x, beehive.transform.position.y - beehiveCell._pathPointHeight, beehive.transform.position.z);
    }

    #endregion

    #region GetWorld/Grid

    public Vector2Int WorldToGrid(Vector3 world)
    {
        float cw = _cellSizes.x;
        float cd = _cellSizes.z;
        float yStep = cd * 0.75f;

        int gy = Mathf.RoundToInt(world.z / yStep);

        float offset = (gy % 2 == 1) ? cw / 2f : 0f;
        int gx = Mathf.RoundToInt((world.x - offset) / cw);

        return new Vector2Int(gx, gy);
    }

    public Vector3 GridToWorld(Vector2Int grid)
    {
        float cw = _cellSizes.x;
        float cd = _cellSizes.z;
        float offset = (grid.y % 2 == 1) ? cw / 2f : 0f;

        return new Vector3(
            grid.x * cw + offset,
            0f,
            grid.y * cd * 0.75f
        );
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        if (_graph == null) return;

        Gizmos.color = Color.greenYellow;

        foreach (var kv in _graph)
        {
            Cell cell = kv.Value;
            if (cell == null) continue;

            if (cell.transform.childCount > 1)
            {
                Transform pathPoint = cell.transform.GetChild(1);
                if (pathPoint != null)
                    Gizmos.DrawSphere(pathPoint.position, 0.15f);
            }
        }
    }
}
