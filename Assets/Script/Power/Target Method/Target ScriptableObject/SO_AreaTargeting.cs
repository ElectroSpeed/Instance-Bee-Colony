using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Targeting/Area Targeting")]
public class SO_AreaTargeting : SO_TargetingBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _radius;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public override IEnumerable<ITarget> GetTargets()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundMask))
        {
            yield return new AreaTarget(hit.point, _radius);
        }
    }
}