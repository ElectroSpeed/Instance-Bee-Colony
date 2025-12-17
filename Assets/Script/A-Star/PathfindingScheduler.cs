using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PathfindingScheduler : MonoBehaviour 
    // Va permettre de faire en sorte d'avoir un gestionnaire de couroutine pour le PF,
    // chaquqe agent va faire une demande au Shreduler pour avancer
{
    public static PathfindingScheduler Instance;

    private Queue<IEnumerator> _jobs = new();
    private const int MAX_STEPS_PER_FRAME = 200; 

    public PathFinding _pathfinding = new PathFinding();

    private void Awake() => Instance = this;

    public void Enqueue(IEnumerator job)
    {
        _jobs.Enqueue(job);
    }

    private void Update()
    {
        int steps = 0;

        while (_jobs.Count > 0 && steps < MAX_STEPS_PER_FRAME)
        {
            if (!_jobs.Peek().MoveNext())
                _jobs.Dequeue();

            steps++;
        }
    }
}
