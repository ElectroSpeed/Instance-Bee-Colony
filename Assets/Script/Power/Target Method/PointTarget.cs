using UnityEngine;

public class PointTarget : ITarget
{
    public Vector3 _position { get; }

    public PointTarget(Vector3 position)
    {
        _position = position;
    }
    
    public bool _isValid() => true;
}
