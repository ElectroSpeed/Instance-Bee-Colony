using System.Collections.Generic;
using UnityEngine;

public interface ITargetingStrategy
{
    IEnumerable<ITarget> GetTargets(Vector3 origin);
}