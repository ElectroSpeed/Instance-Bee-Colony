using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    [Header("Map settings")]
    [SerializeField] private GameObject testTile;
    [SerializeField] public int length = 10;
    [SerializeField] public int width = 10;
    [SerializeField] private List<int> holes = new List<int>();

    [Header("Obstacles settings")]
    [Range(0f, 1f)] public float obstacleRate = 0.2f;
    public bool generateRandomObstacles = true;
    public List<Vector2Int> obstacles = new List<Vector2Int>();

    [Header("Runtime")]
    public List<GameObject> tiles = new List<GameObject>();
    public List<Vector2> tilePositions = new List<Vector2>();

    public Dictionary<Vector2Int, Cell> graph = new Dictionary<Vector2Int, Cell>();


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if (generateRandomObstacles)
            GenerateRandomObstacles();

        GenerateMap();
    }

    void GenerateRandomObstacles()
    {
        obstacles.Clear();

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                int index = y * length + x;

                if (holes.Contains(index))
                    continue;

                if (Random.value < obstacleRate)
                {
                    obstacles.Add(new Vector2Int(x, y));
                }
            }
        }

        Debug.Log("Obstacles generated : " + obstacles.Count);
    }


    public void GenerateMap()
    {
        foreach (var t in tiles)
            if (t != null) Destroy(t);

        tiles.Clear();
        tilePositions.Clear();
        graph.Clear();

        GameObject tile;
        int index = 0;

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                Vector3 pos = new Vector3((y % 2 == 1 ? 1f : 0f) + x * 2f, 0f, y * 2f);

                if (!holes.Contains(index))
                {
                    tile = Instantiate(testTile, pos, Quaternion.identity, transform);
                    tiles.Add(tile);
                    tilePositions.Add(new Vector2(pos.x, pos.z));
                }
                else
                {
                    tiles.Add(null);
                    tilePositions.Add(new Vector2(-1, -1));
                }

                index++;
            }
        }

        BuildGraph();
    }


    void BuildGraph()
    {
        graph.Clear();
        int index = 0;

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (!holes.Contains(index))
                {
                    Vector2Int gridPos = new Vector2Int(x, y);

                    bool isWalkable = !obstacles.Contains(gridPos);

                    graph[gridPos] = new Cell(x, y, isWalkable);
                }
                index++;
            }
        }
    }


    public Vector2Int WorldToGrid(Vector3 world)
    {
        int gy = Mathf.FloorToInt(world.z / 2f + 0.5f);
        float offset = (gy % 2 == 1) ? 1f : 0f;
        int gx = Mathf.FloorToInt((world.x - offset) / 2f + 0.5f);

        return new Vector2Int(gx, gy);
    }

    public Vector3 GridToWorld(Vector2Int grid)
    {
        float offset = (grid.y % 2 == 1) ? 1f : 0f;
        return new Vector3(grid.x * 2f + offset, 0f, grid.y * 2f);
    }


    private void OnDrawGizmosSelected()
    {
        if (graph == null) return;

        foreach (var kv in graph)
        {
            Vector3 w = GridToWorld(kv.Key) + Vector3.up * 0.1f;

            if (kv.Value.isWalkable)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.blue;

            Gizmos.DrawSphere(w, 0.15f);
        }
    }
}
