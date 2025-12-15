using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Effects/RainEffect")]
public class RainEffectBehaviour : SO_EffectBehaviour
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
        _environment.SetHumidity(_environment._humidity + _intensity);
    }
}