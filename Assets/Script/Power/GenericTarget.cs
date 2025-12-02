using UnityEngine;

public class GenericTarget : ITarget
{
    public Vector3 _position { get; private set; }
    public GameObject _entity { get; private set; }

    public GenericTarget(GameObject obj)
    {
        _entity = obj;
        _position = obj.transform.position;
    }
    
    public Transform GetTransform()
    {
        return _entity != null ? _entity.transform : null;
    }

    public Vector3 GetPosition()
    {
        return _position;
    }

    public GameObject GetEntity()
    {
        return _entity;
    }
}