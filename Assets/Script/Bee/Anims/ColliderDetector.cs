using System;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ColliderDetector : MonoBehaviour
{
    internal Action<Collision> onTriggerEnterFunction= x => { return; } ;
    internal Action<Collision> onTriggerExitFunction= x => { return; } ;

    private void OnCollisionEnter(Collision other)
    {
        onTriggerEnterFunction?.Invoke(other);
    }

    private void OnCollisionExit(Collision other)
    {
        onTriggerExitFunction?.Invoke(other);
    }
}
