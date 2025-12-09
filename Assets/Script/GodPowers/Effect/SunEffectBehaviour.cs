using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Effects/SunEffect")]
public class SunEffectBehaviour : SO_EffectBehaviour
{
    [SerializeField] private float _intensity;

    public override void ApplyEffect(IEnumerable<ITarget> targets)
    {
        EnvironmentManager environment = Object.FindFirstObjectByType<EnvironmentManager>();

        if (environment == null)
        {
            return;
        }
        
        SunEffect sun = new SunEffect(environment, _intensity);
        sun.ApplyEffect(null);
    }
}