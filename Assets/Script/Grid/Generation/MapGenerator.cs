using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class MapGenerator : MonoBehaviour
{

    [Header("Terrain")]
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;
    
    
    [Tooltip("the number of octaves, an octave is the level of detail in your noise map, see it as the number of noise map superposed on top of each others")]
    [SerializeField] private int octaves;
    
    [Tooltip("How much the noise will be zoomed in, the bigger the value, the bigger the zoom")]
    [SerializeField] private float scale;
    
    
    [Range(0,1)]
    [Tooltip("A value between 0 and 1, determines how much each octave contributes to the overall shap")]
    [SerializeField] private float persistance
        ;
    [Tooltip("number that determines how much detail is added or removed at each octave")]
    [SerializeField] private float lacunarity;
    
    [Min(1)]
    [Tooltip("The multiplier for the base scale of the noise map, for having a less flat terrain")]
    [SerializeField] private float heightMultiplier=1;

    [Tooltip("Change the falloff of the noise map based on the curve value")]
    [SerializeField] private AnimationCurve heightCurve;
    [Space(5)]
    
    [Header("Heat Overlay")]
    
    [SerializeField] private int heatOctaves;
    
    [SerializeField] private float heatScale;
    
    [Range(0,1)]
    [SerializeField] private float heatPersistance;
    
    [SerializeField] private float heatLacunarity;
    
    [SerializeField] private AnimationCurve heatCurve;

    
    [SerializeField] private bool impactedByElevation = true;
    [Tooltip("How much the heightmap (the elevation of the terrain) influence the heat map")]
    [SerializeField] private float elevationMultiplier = 1;
    
    
    [Header("Obstacles settings")]
    [Range(0f, 1f)] public float obstacleRate = 0.2f;
    public bool generateRandomObstacles = true;
    public List<Vector2Int> obstacles = new List<Vector2Int>();
    [SerializeField] private List<int> holes = new List<int>();

    [Header("Pathfinding Related")] [SerializeField]
    private float pathMultiplier = 1;

    
    [Space(10)]
    [SerializeField] private GameObject cellPrefab;
    List<GameObject> cells = new List<GameObject>();
    public Dictionary<Vector2Int, Cell> graph = new Dictionary<Vector2Int, Cell>();



    private void Start()
    {
        int heightSeed = Random.Range(0, int.MaxValue);
        int heatSeed = Random.Range(0, int.MaxValue);
        
        float[,] heightMap=Noise.GenerateNoiseMap(width, height, heightSeed,scale,octaves,persistance,lacunarity,Vector2.zero,Noise.NormalizeMode.Global);
        float[,] heatMap=Noise.GenerateNoiseMap(width, height, heatSeed,heatScale,heatOctaves,heatPersistance,heatLacunarity,Vector2.zero);
        
        Debug.Log("Seed of Height Map : " + heightSeed);
        Debug.Log("Seed of Heat Map : " + heatSeed);
        
        if (generateRandomObstacles)
            GenerateRandomObstacles();
        
        GenerateMap();
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                heightMap[i, j] = heightCurve.Evaluate(heightMap[i, j]);

                heatMap[i, j] = heatCurve.Evaluate(heatMap[i, j]);

                if (impactedByElevation)
                {

                    heatMap[i, j] *= Mathf.Clamp01(1 - heightMap[i, j] * elevationMultiplier);

                }

                //cells[i * cells.Count + j].transform.position+=new Vector3(0, (heightMap[i,j]*heightMultiplier)/2, 0);

                if (heightMap[i, j] < 0.0001f)
                {
                    heightMap[i, j] = 0.001f;
                }


                if (!holes.Contains(i * width + j))
                {
                    //the child(0) is the hexagon mesh
                    //TODO : Change Cell prefab with the definitive mesh
                    cells[i * width + j].transform.GetChild(0).localScale = new Vector3(
                        transform.GetChild(0).localScale.x, transform.GetChild(0).localScale.y,
                        heightMap[i, j] * heightMultiplier);
                    
                    //TODO : Create an overlay instead of changing directly the mesh color
                    cells[i * width + j].transform.GetChild(0).GetComponent<MeshRenderer>().material.color =
                        new Color(heatMap[i, j], 1 - heatMap[i, j], 1 - heatMap[i, j]);
                    
                    //cells[i * width + j].transform.position+=new Vector3(0, (heightMap[i,j]*heightMultiplier)/2, 0);
                }
            }
        }
        BuildGraph(heightMap);
        
    }

    public void GenerateMap()
    {
        
        int index = 0;
        for (int y = 0; y < width; y++)
        {
            
            for (int x = 0; x < height; x++)
            {
                Vector3 pos = new Vector3((y % 2 * .5f * 2) + x * 2, 
                    0, 
                    y * 2);
                if (!holes.Contains(index))
                {
                    
                    cells.Add(Instantiate(cellPrefab,pos,Quaternion.Euler(0,0,0),gameObject.transform));
                }
                else
                {
                    cells.Add(null);
                }
            }
            index++;
        }
        
    }
    
    void BuildGraph(float[,] heightMap)
    {
        graph.Clear();
        int index = 0;

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                if (!holes.Contains(index))
                {
                    Vector2Int gridPos = new Vector2Int(x, y);

                    bool isWalkable = !obstacles.Contains(gridPos);
                    
                    
                    graph[gridPos] = new Cell(x, y, heightMap[y, x] * pathMultiplier);
                }
                index++;
            }
        }
    }
    
    void GenerateRandomObstacles()
    {
        obstacles.Clear();

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                int index = y * width + x;

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


    // private void OnDrawGizmosSelected()
    // {
    //     if (graph == null) return;
    //
    //     foreach (var kv in graph)
    //     {
    //         Vector3 w = GridToWorld(kv.Key) + Vector3.up * 10f;
    //
    //         if (kv.Value.isWalkable)
    //             Gizmos.color = Color.green;
    //         else
    //             Gizmos.color = Color.blue;
    //
    //         Gizmos.DrawSphere(w, 0.15f);
    //     }
    // }
}
