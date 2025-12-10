using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTester : MonoBehaviour
{
    private PathFinding pathFinding;

    [Tooltip("Transform du point de départ")]
    public Transform _startTransform;

    [Tooltip("Transform du point d'arrivée")]
    public Transform _endTransform;

    private List<Vector3> _pathPositions = null;

    private void Awake()
    {
        pathFinding = new PathFinding();
    }

    private void Start()
    {
        StartCoroutine(DelayedFindPath(5f));
    }

    private IEnumerator DelayedFindPath(float delay)
    {
        yield return new WaitForSeconds(delay);
        CalculatePath();
    }

    private void CalculatePath()
    {
        if (_startTransform == null || _endTransform == null)
        {
            Debug.LogWarning("StartTransform or EndTransform is not assigned!");
            _pathPositions = null;
            return;
        }

        Vector3 startPoint = _startTransform.position;
        Vector3 endPoint = _endTransform.position;

        _pathPositions = pathFinding.FindPathPositions(startPoint, endPoint);

        if (_pathPositions == null)
            Debug.LogWarning("No path found.");
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (_startTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_startTransform.position, 0.3f);
        }

        if (_endTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_endTransform.position, 0.3f);
        }

        if (_pathPositions != null && _pathPositions.Count > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < _pathPositions.Count - 1; i++)
            {
                Gizmos.DrawLine(_pathPositions[i], _pathPositions[i + 1]);
            }
        }
    }
}