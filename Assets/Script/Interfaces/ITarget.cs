using UnityEngine;

public interface ITarget
{
    Transform GetTransform();
    Vector3 GetPosition();
    GameObject GetEntity();
}