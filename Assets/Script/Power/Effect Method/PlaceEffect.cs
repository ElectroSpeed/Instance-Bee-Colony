using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Effects/PlaceObjectEffect")]
public class PlaceEffect : SO_EffectBehaviour
{
    [SerializeField] private GameObject _prefabToPlace;

    public override void ApplyEffect(IEnumerable<ITarget> targets)
    {
        if (_prefabToPlace == null || targets == null)
            return;

        foreach (var target in targets)
        {
            if (target == null || !target._isValid())
                continue;

            Vector3 pos = target._position;

            Instantiate(_prefabToPlace, pos, Quaternion.identity);

            if (target is EntityTarget entityTarget)
            {
                GameObject entity = entityTarget._entity;

                if (entity != null && entity.name.Contains("RaycastTarget"))
                {
                    Object.Destroy(entity);
                }
            }
        }
    }
}
