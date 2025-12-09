 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTester : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    [SerializeField] MapGenerator mapGenerator;
    
    private PathFinding pathfinding;
    private List<Cell> path;

    private void Start()
    {
        pathfinding = new PathFinding(mapGenerator);
        StartCoroutine(TestAfterDelay());
    }

    private IEnumerator TestAfterDelay()
    {
        yield return new WaitForSeconds(10f);

        path = pathfinding.FindPath(startPoint.position, endPoint.position);

        if (path == null)
            Debug.Log("No path found");
        else
            Debug.Log("Path length : " + path.Count);
    }

    private void OnDrawGizmos()
    {
        if (path == null || mapGenerator == null) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 worldPos = mapGenerator.GridToWorld(path[i].position);
            Gizmos.DrawSphere(worldPos + Vector3.up * 5f, 0.2f);

            if (i < path.Count - 1)
            {
                Vector3 nextWorldPos = mapGenerator.GridToWorld(path[i + 1].position);
                Gizmos.DrawLine(
                    worldPos + Vector3.up * 5f,
                    nextWorldPos + Vector3.up * 5f
                );
            }
        }
    }
}