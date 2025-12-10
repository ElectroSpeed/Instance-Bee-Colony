using System;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ColliderDetector : MonoBehaviour
{
    internal Action<Collision> _onTriggerEnterFunction= x => { return; } ;
    internal Action<Collision> _onTriggerExitFunction= x => { return; } ;

    private void OnCollisionEnter(Collision other)
    {
        _onTriggerEnterFunction?.Invoke(other);
    }

    private void OnCollisionExit(Collision other)
    {
        _onTriggerExitFunction?.Invoke(other);
    }
}
