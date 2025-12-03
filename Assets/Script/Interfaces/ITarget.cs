using UnityEngine;
public interface ITarget
{
    Vector3 _position { get; }
    bool _isValid();
}