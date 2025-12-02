using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "GodGame/Targeting/RaycastTargeting")]
public class RaycastTargeting : SO_TargetingBehaviour
{
    [SerializeField] private LayerMask _groundLayer = ~0;
    [SerializeField] private float _maxDistance = 100f;

    public override IEnumerable<ITarget> GetTargets(Vector3 origin)
    {
        List<ITarget> targets = new List<ITarget>();
        Camera cam = Camera.main;

        if (cam == null)
        {
            return targets;
        }
        
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = cam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _groundLayer))
        {
            GameObject dummy = new GameObject("RaycastTarget");
            dummy.transform.position = hit.point;
            targets.Add(new GenericTarget(dummy));

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 1f);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * _maxDistance, Color.red, 1f);
        }

        return targets;
    }
}