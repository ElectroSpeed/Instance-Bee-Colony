using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Effects/RainEffect")]
public class RainEffectBehaviour : SO_EffectBehaviour
{
    [SerializeField] private float _intensity;

    public override void ApplyEffect(IEnumerable<ITarget> targets)
    {
        EnvironmentManager environment = Object.FindFirstObjectByType<EnvironmentManager>();

        if (environment == null)
        {
            return;
        }
        
        RainEffect rain = new RainEffect(environment, _intensity);
        rain.ApplyEffect(null);
    }
}