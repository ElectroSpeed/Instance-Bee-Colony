using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "GodGame/Targeting/Entity Targeting")]
public class SO_EntityTargeting : SO_TargetingBehaviour
{
    [SerializeField] private LayerMask _entityMask;
    
    private Camera _camera;

    public override IEnumerable<ITarget> GetTargets()
    {
        _camera =  Camera.main;
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _entityMask))
        {
            yield return new EntityTarget(hit.collider.gameObject);
        }
    }
}