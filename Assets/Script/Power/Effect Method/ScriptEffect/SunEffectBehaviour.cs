using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Effects/SunEffect")]
public class SunEffectBehaviour : SO_EffectBehaviour
{
    [SerializeField] private EnvironmentManager _environment;
    [SerializeField] private float _intensity;

    public override void ApplyEffect(IEnumerable<ITarget> targets)
    {
        _environment = Object.FindFirstObjectByType<EnvironmentManager>();

        if (_environment == null)
        {
            return;
        }
        _environment._isPowerActive = true;
        _environment.SetSunlight(_environment._sunlight + _intensity);
    }
}