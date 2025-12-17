using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Effects/SunEffect")]
public class SunEffectBehaviour : SO_EffectBehaviour
{
    private EnvironmentManager _environment;
    [SerializeField] private float _intensity;

    public override void ApplyEffect(IEnumerable<ITarget> targets)
    {
        _environment = EnvironmentManager.Instance;

        if (_environment == null)
        {
            return;
        }
        _environment._isPowerActive = true;
        _environment.SetSunlight(_environment._sunlight + _intensity);
    }
}