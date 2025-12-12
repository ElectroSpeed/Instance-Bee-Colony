using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObstaclesGenerator :  MonoBehaviour
{
    public bool GenerateEnvironment(Vector3 point, float obstacleChance, float meshPos, out Vector3 newPos)
    {
        float a =Random.Range(0f, 100f);
        if (a >= obstacleChance)
        {
            
            

            
            Vector3 pos = point;
            pos.y -= meshPos;
            newPos = pos;
            return true;
        }
        newPos = Vector3.zero;
        return false;
    }
}
