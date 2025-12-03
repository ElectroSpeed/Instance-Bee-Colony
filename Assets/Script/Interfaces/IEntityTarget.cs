using UnityEngine;

public interface IEntityTarget : ITarget
{
    GameObject _entity { get; }
    Transform _transform { get; }
}