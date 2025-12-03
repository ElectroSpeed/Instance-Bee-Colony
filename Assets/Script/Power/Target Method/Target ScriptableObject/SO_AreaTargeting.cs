using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Targeting/Area Targeting")]
public class SO_AreaTargeting : SO_TargetingBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _radius;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    public override IEnumerable<ITarget> GetTargets(Vector3 origin)
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundMask))
        {
            yield return new AreaTarget(hit.point, _radius);
        }
    }
}