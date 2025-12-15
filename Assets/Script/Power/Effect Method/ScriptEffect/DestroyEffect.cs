using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Effects/DestroyEffect")]
public class DestroyEffect : SO_EffectBehaviour
{
    public override void ApplyEffect(IEnumerable<ITarget> targets)
    {
        if (targets == null)
            return;

        foreach (ITarget target in targets)
        {
            if (target == null || !target._isValid())
                continue;

            if (target is EntityTarget entityTarget)
            {
                GameObject entity = entityTarget._entity;

                if (entity != null)
                {
                    Object.Destroy(entity.transform.parent.gameObject);
                }
            }
        }
    }
}