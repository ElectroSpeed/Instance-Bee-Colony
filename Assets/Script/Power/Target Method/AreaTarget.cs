using UnityEngine;

public class AreaTarget : ITarget
{
    public Vector3 _position { get; }
    public float _radius { get; }

    public AreaTarget(Vector3 position, float radius)
    {
        _position = position;
        _radius = radius;
    }
    
    public bool _isValid()
    {
        return _radius > 0;
    }
}

