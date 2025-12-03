using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Targeting/Point Targeting")]
public class SO_PointTargeting : SO_TargetingBehaviour
{
    [SerializeField] private LayerMask _groundMask;

    public override IEnumerable<ITarget> GetTargets(Vector3 origin)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundMask))
        {
            yield return new PointTarget(hit.point);
        }
    }
}