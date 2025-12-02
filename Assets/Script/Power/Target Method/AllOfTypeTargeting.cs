using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Targeting/AllOfTypeTargeting")]
public class AllOfTypeTargeting : SO_TargetingBehaviour
{
    [SerializeField] private string _tagFilter;
    [SerializeField] private bool _useTagFilter = false;

    public override IEnumerable<ITarget> GetTargets(Vector3 origin)
    {
        List<ITarget> targets = new List<ITarget>();

        return targets;
    }
}