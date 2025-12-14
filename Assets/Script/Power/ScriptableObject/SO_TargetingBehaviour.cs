using System.Collections.Generic;
using UnityEngine;

public abstract class SO_TargetingBehaviour : ScriptableObject
{
    public abstract IEnumerable<ITarget> GetTargets();
}