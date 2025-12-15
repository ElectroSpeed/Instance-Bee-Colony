using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "GodGame/Targeting/Point + Nearest Entity")]
public class SO_PointAndNearestEntityTargeting : SO_TargetingBehaviour
{
    [Header("Raycast")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _maxDistance = 1000f;

    [Header("Entity Scan")]
    [SerializeField] private float _scanRadius = 5f;
    [SerializeField] private LayerMask _entityMask;
    
    private Camera _camera;

    public override IEnumerable<ITarget> GetTargets()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            yield break;
        
        _camera = Camera.main;
        if (_camera == null)
            yield break;

        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _groundMask))
            yield break;

        Vector3 hitPoint = hit.point;
        
        PointTarget pointTarget = new PointTarget(hitPoint);

        if (pointTarget._isValid())
            yield return pointTarget;

        Collider[] colliders = Physics.OverlapSphere(hitPoint, _scanRadius, _entityMask);

        if (colliders.Length == 0)
            yield break;

        GameObject closestEntity = null;
        float minDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            float distance = Vector3.Distance(hitPoint, col.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestEntity = col.gameObject;
            }
        }

        if (closestEntity != null)
        {
            EntityTarget entityTarget = new EntityTarget(closestEntity);

            if (entityTarget._isValid())
                yield return entityTarget;
        }
    }
}
