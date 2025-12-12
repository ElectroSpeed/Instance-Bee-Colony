using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [Header("Map settings")]
    [SerializeField] private Cell _tilePrefab;
    [SerializeField] private int _gridLength = 10;
    [SerializeField] private int _gridWidth = 10;

    [Header("Perlin Noise Settings")]
    public int _seed = 12345;
    public float _scale = 20f;
    public int _octaves = 4;
    [Range(0f, 1f)] public float _persistance = 0.5f;
    public float _lacunarity = 2f;
    public Vector2 _noiseOffset = Vector2.zero;
    public Noise.NormalizeMode _normalizeMode = Noise.NormalizeMode.Local;

    [Header("Terrain settings")] 
    public AnimationCurve _mapCurve;
    public float _heightMultiplier = 3f;
    public Gradient _colorGradient;

    public Dictionary<Vector2Int, Cell> _graph = new Dictionary<Vector2Int, Cell>();
    public Vector3 _cellSizes = Vector3.zero;
    
    [SerializeField] private List<Mesh> _obstacles;
    
    [Range(0,100)]
    [SerializeField] private float _obstacleChance;
    [SerializeField] GameObject _environmentObstaclePrefab;
    
    EnvironmentObstaclesGenerator _environmentObstaclesGenerator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_tilePrefab == null)
        {
            return;
        }

        Cell _sizeTestCell = Instantiate(_tilePrefab, Vector3.zero, _tilePrefab.transform.rotation, transform);
        Renderer _sizeTestCellRenderer = _sizeTestCell.GetComponentInChildren<Renderer>();
        if (_sizeTestCellRenderer == null)
        {
            Debug.LogError("MapGenerator: Aucun Renderer trouvé sur le prefab de tuile.");
            Destroy(_sizeTestCell.gameObject);
            return;
        }

        Vector3 worldSize = _sizeTestCellRenderer.bounds.size;
        _cellSizes = worldSize;

        Destroy(_sizeTestCell.gameObject);
    }


    private void Start()
    {
        _environmentObstaclesGenerator = GetComponent<EnvironmentObstaclesGenerator>();
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

        float[,] noiseMap = Noise.GenerateNoiseMap(_gridLength, _gridWidth, _seed, _scale, _octaves, _persistance, _lacunarity, _noiseOffset, _normalizeMode);
        
        float cellWidth = _cellSizes.x;
        float cellDepth = _cellSizes.z;

        float xStep = cellWidth;
        float yStep = cellDepth * 0.75f;

        for (int y = 0; y < _gridWidth; y++)
        {
            for (int x = 0; x < _gridLength; x++)
            {
                float offset = (y % 2 == 1) ? cellWidth / 2f : 0f;

                Vector3 pos = new Vector3(x * xStep + offset, 0f, y * yStep);

                Cell mapCell = Instantiate(_tilePrefab, pos, _tilePrefab.transform.rotation, transform);
                
                mapCell._position = new Vector2Int(x, y);

                noiseMap[x,y]= _mapCurve.Evaluate(noiseMap[x,y]);
                float noiseValue = noiseMap[x, y];
                //float noiseValue = _mapCurve.Evaluate(noiseMap[x,y]);

                Transform mapCellMesh = mapCell.transform.GetChild(0).GetChild(0);
                
                if (mapCellMesh != null)
                {
                    float height = Mathf.Max(0.1f, noiseValue * _heightMultiplier);
                    mapCell._height = height;

                    Vector3 meshScale = mapCellMesh.localScale;

                    meshScale.z = height / 2f;
                    mapCellMesh.localScale = meshScale;

                    Vector3 meshPosition = mapCellMesh.localPosition;
                    meshPosition.y = height / 4f;
                    mapCellMesh.localPosition = meshPosition;

                    
                    
                    
                    
                    Transform mapCellPathPoint = mapCell.transform.GetChild(1);
                    
                    if (mapCellPathPoint != null)
                    {
                        Vector3 mapCellPathPointPosition = mapCellPathPoint.localPosition;
                        float mapCellPathPointPositionY = meshPosition.y + (height / 4f);
                        mapCellPathPointPosition.y = mapCellPathPointPositionY + mapCell._pathPointHeight;
                        mapCellPathPoint.localPosition = mapCellPathPointPosition;
                    }
                    Mesh obstacleMesh = _obstacles[Random.Range(0, _obstacles.Count)];
                    float a = obstacleMesh.bounds.size.y;

                    Vector3 b = mapCell._pathPoint.position;
                    
                    b.y -= mapCell._pathPointHeight;
                    
                    if (_environmentObstaclesGenerator.GenerateEnvironment(b, _obstacleChance, a,
                            out Vector3 obstaclePos))
                    {
                        GameObject newObstacle=Instantiate(_environmentObstaclePrefab, obstaclePos, Quaternion.identity);
                        newObstacle.GetComponentInChildren<MeshFilter>().mesh = obstacleMesh;
                        mapCell._isWalkable = false;
                    }
                }
                
                
                Renderer mapCellRenderer = mapCell.GetComponentInChildren<Renderer>();
                if (mapCellRenderer != null)
                {
                    
                    mapCellRenderer.material.color = _colorGradient.Evaluate(noiseValue);
                }

                _graph.Add(new Vector2Int(x, y), mapCell);
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

        Gizmos.color = Color.greenYellow;
    
        foreach (var kv in _graph)
        {
            Cell cell = kv.Value;
            if (cell == null) continue;
            
            Transform pathPoint = null;
            if (cell.transform.childCount > 1)
                pathPoint = cell.transform.GetChild(1);

            if (pathPoint != null)
            {
                Vector3 worldPos = pathPoint.position;

                Gizmos.DrawSphere(worldPos, 0.15f);
            }
        }
    }

}