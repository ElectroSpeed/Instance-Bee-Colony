using System.Collections.Generic;
using UnityEngine;

public abstract class SO_EffectBehaviour : ScriptableObject
{
    public abstract void ApplyEffect(IEnumerable<ITarget> targets);
}