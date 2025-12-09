using System.Collections.Generic;

public class RainEffect : IEffect
{
    private EnvironmentManager _environment;
    private float _intensity;

    public RainEffect(EnvironmentManager environment, float intensity)
    {
        _environment = environment;
        _intensity = intensity;
    }

    public void ApplyEffect(IEnumerable<ITarget> targets)
    {
        if (_environment == null)
        {
            return;
        }
        _environment.SetHumidity(_environment._humidity + _intensity);
    }
}