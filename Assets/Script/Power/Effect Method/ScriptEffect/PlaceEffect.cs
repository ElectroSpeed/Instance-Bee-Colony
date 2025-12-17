using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Effects/PlaceObjectEffect")]
public class PlaceEffect : SO_EffectBehaviour
{
    [SerializeField] private GameObject _prefabToPlace;
    
    bool IsPositionOccupied(Vector3 pos)
    {
        return GameObject.FindGameObjectsWithTag("Flower")
            .Any(o => Vector3.Distance(o.transform.position, pos) < 1f);
    }

    public override void ApplyEffect(IEnumerable<ITarget> targets)
    {
        if (_prefabToPlace == null || targets == null)
        {
            return;
        }

        foreach (ITarget target in targets)
        {
            if (target == null || !target._isValid())
            {
                continue;
            }

            Vector3 pos = target._position;
            
            if (!IsPositionOccupied(pos))
                continue;

            Instantiate(_prefabToPlace, pos, Quaternion.identity);

            if (target is EntityTarget entityTarget)
            {
                GameObject entity = entityTarget._entity;

                if (entity != null && entity.name.Contains("RaycastTarget"))
                {
                    Destroy(entity);
                }
            }
        }
    }
}
