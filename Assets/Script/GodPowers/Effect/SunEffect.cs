using System.Collections.Generic;

public class SunEffect : IEffect
{
    private EnvironmentManager _environment;
    private float _intensity;

    public SunEffect(EnvironmentManager environment, float intensity)
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
        _environment.SetSunlight(_environment._sunlight + _intensity);
    }
}