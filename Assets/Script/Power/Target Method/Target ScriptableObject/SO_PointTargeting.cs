using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "GodGame/Targeting/Point Targeting")]
public class SO_PointTargeting : SO_TargetingBehaviour
{
    [Header("Raycast")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _maxDistance = 1000f;
    [SerializeField] private float _objectPlaceHeight = 1f;
    
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public override IEnumerable<ITarget> GetTargets()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _groundMask))
        {
            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell != null && cell._pathPoint != null)
            {
                Vector3 point = cell._pathPoint.position;
                point.y -= cell._pathPointHeight;
                point.y += _objectPlaceHeight;

                yield return new PointTarget(point);
                yield break;
            }

            yield return new PointTarget(hit.point);
        }
    }
}