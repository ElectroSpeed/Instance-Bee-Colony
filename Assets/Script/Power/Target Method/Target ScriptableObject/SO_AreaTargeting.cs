using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "GodGame/Targeting/Area Targeting")]
public class SO_AreaTargeting : SO_TargetingBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _radius;

    private Camera _camera;

    public override IEnumerable<ITarget> GetTargets()
    {
        _camera = Camera.main;
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundMask))
        {
            yield return new AreaTarget(hit.point, _radius);
        }
    }
}