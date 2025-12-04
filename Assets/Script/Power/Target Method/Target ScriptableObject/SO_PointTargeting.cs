using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Targeting/Point Targeting")]
public class SO_PointTargeting : SO_TargetingBehaviour
{
    [Header("Raycast")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _maxDistance = 1000f;

    public override IEnumerable<ITarget> GetTargets(Vector3 origin)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _groundMask))
        {
            yield return new PointTarget(hit.point);
        }
    }
}