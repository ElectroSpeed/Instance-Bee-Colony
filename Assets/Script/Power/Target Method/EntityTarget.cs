using UnityEngine;

public class EntityTarget : ITarget
{
    public GameObject _entity { get; }
    public Transform _transform => _entity ? _entity.transform : null;
    public Vector3 _position => _entity ? _entity.transform.position : Vector3.zero;

    public EntityTarget(GameObject entity)
    {
        _entity = entity;
    }
    
    public bool _isValid()
    {
        return _entity != null && _entity.activeInHierarchy && _entity.scene.IsValid();
    }

}