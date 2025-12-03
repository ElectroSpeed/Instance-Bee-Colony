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
    
    [SerializeField] private int octaves;
    
    [SerializeField] private float scale;
    
    [Range(0,1)]
    [SerializeField] private float persistance;
    [SerializeField] private float lacunarity;
    
    [Range(1,50)]
    [SerializeField] private float heightMultiplier=1;

    [SerializeField] private AnimationCurve heightCurve;
    
    [Header("Heat Overlay")]
    
    [SerializeField] private int heatOctaves;
    
    [SerializeField] private float heatScale;
    
    [Range(0,1)]
    [SerializeField] private float heatPersistance;
    [SerializeField] private float heatLacunarity;
    
    [SerializeField] private AnimationCurve heatCurve;
    
    [SerializeField] private GameObject cellPrefab;
    Dictionary<int, GameObject> cells = new Dictionary<int, GameObject>();


    private void Start()
    {
        int heightSeed = Random.Range(0, int.MaxValue);
        int heatSeed = Random.Range(0, int.MaxValue);
        float[,] heightMap=Noise.GenerateNoiseMap(width, height, heightSeed,scale,octaves,persistance,lacunarity,Vector2.zero,heightCurve,Noise.NormalizeMode.Global);
        float[,] heatMap=Noise.GenerateNoiseMap(width, height, heatSeed,heatScale,heatOctaves,heatPersistance,heatLacunarity,Vector2.zero,heatCurve);
        Debug.Log("Seed of Height Map : " + heightSeed);
        Debug.Log("Seed of Heat Map : " + heatSeed);
        GenerateMap(new Vector2(cellPrefab.transform.localScale.x, cellPrefab.transform.localScale.y));
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                heightMap[i, j] = heightCurve.Evaluate(heightMap[i, j]);
                heatMap[i, j] = heatCurve.Evaluate(heatMap[i, j]);
                cells[i * cells.Count + j].transform.position+=new Vector3(0, (heightMap[i,j]*heightMultiplier)/2, 0);
                if (heightMap[i, j] < 0.0001f)
                {
                    heightMap[i, j] = 0.001f;
                }
                cells[i * cells.Count + j].transform.GetChild(0).localScale=new Vector3(transform.GetChild(0).localScale.x, transform.GetChild(0).localScale.y,heightMap[i, j]*heightMultiplier);
                cells[i * cells.Count + j].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(heatMap[i,j], 1-heatMap[i,j],1-heatMap[i,j]);
            }
        }
        
    }

    public void GenerateMap(Vector2 cellSize)
    {
        for (int y = 0; y < width; y++)
        {
            
            for (int x = 0; x < height; x++)
            {
                Vector3 pos = new Vector3((y % 2 * .5f * 2) + x * 2, 
                    0, 
                    y * 2);
                cells.Add(y * (width*height) + x,Instantiate(cellPrefab,pos,Quaternion.Euler(0,0,0),gameObject.transform));
            }
        }
    }
}
