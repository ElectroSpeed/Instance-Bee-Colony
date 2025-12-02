using System.Collections.Generic;
using UnityEngine;

public abstract class SO_TargetingBehaviour : ScriptableObject, ITargetingStrategy
{
    public abstract IEnumerable<ITarget> GetTargets(Vector3 origin);
}