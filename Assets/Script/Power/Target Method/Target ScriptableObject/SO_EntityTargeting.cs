using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Targeting/Entity Targeting")]
public class SO_EntityTargeting : SO_TargetingBehaviour
{
    [SerializeField] private LayerMask _entityMask;
    
    private Camera _cam;

    private void Awake()
    {
        _cam =  Camera.main;
    }

    public override IEnumerable<ITarget> GetTargets(Vector3 origin)
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _entityMask))
        {
            yield return new EntityTarget(hit.collider.gameObject);
        }
    }
}