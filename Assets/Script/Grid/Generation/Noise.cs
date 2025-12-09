using UnityEngine;
using System.Collections;


public static class Noise
{
    
    /// <summary>
    /// Local is used with one chunk, Global is used with multiple chunks
    /// </summary>
    public enum NormalizeMode{Local,Global}
    

    /// <param name="mapWidth"></param>
    /// <param name="mapHeight"></param>
    /// <param name="seed"></param>
    /// <param name="scale">How much the noise will be zoomed in, the bigger the value, the bigger the zoom</param>
    /// <param name="octaves">the number of octaves, an octave is the level of detail in your noise map, see it as the number of noise map superposed on top of each others</param>
    /// <param name="persistance">A value between 0 and 1, determines how much each octave contributes to the overall shape</param>
    /// <param name="lacunarity">number that determines how much detail is added or removed at each octave</param>
    /// <param name="offset"></param>
    /// <param name="normalizeMode"></param>
    /// <returns></returns>
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset,NormalizeMode normalizeMode=NormalizeMode.Local) {
        
        float[,] noiseMap = new float[mapWidth,mapHeight];
        
        System.Random rndValue = new System.Random (seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        
        float maxPossibleHeight = 0;
        //float minPossibleHeight = 0;
        
        float amplitude = 1;
        float frequency = 1;
        
        //Compute octaves value
        for (int i = 0; i < octaves; i++) {
            float offsetX = rndValue.Next (-100000, 100000) + offset.x;
            float offsetY = rndValue.Next (-100000, 100000) - offset.y;
            octaveOffsets [i] = new Vector2 (offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if (scale <= 0) {
            scale = 0.0001f;
        }

        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;
        
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;


        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
        
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    
                    float sampleX = (x-halfWidth+ octaveOffsets[i].x) / scale * frequency ;
                    float sampleY = (y-halfHeight+ octaveOffsets[i].y) / scale * frequency ;

                    float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
                    
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                
                // Keep trace of min and max values
                if (noiseHeight > maxLocalNoiseHeight) {
                    maxLocalNoiseHeight = noiseHeight;
                } else if (noiseHeight < minLocalNoiseHeight) {
                    minLocalNoiseHeight = noiseHeight;
                }
                noiseMap [x, y] = noiseHeight;
            }
        }

        // Normalization
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap [x, y] = Mathf.InverseLerp (minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap [x, y]);
                }
                else
                {
                    float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight);
                    noiseMap[x,y]=Mathf.Clamp(normalizedHeight,0,int.MaxValue);
                }

            }
            
        }

        return noiseMap;
    }
    
}