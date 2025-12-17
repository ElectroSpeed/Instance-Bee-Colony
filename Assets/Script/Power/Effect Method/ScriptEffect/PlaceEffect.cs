using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/Effects/PlaceObjectEffect")]
public class PlaceEffect : SO_EffectBehaviour
{
    [SerializeField] private GameObject _prefabToPlace;
    
    List<Flower> _flowers = Locator<Flower>.Get<Flower>();
    List<GameObject> _obstacles = Locator<GameObject>.Get<GameObject>();
    List<Beehive> _beehives = Locator<Beehive>.Get<Beehive>();
    
    bool IsPositionOccupiedByFlower(Vector3 pos)
    {
        foreach (Flower flower in _flowers)
        {
            if (Vector3.Distance(flower.transform.position, pos) < 1f)
            {
                return true;
            }
        }
        return false;
    }
    
    bool IsPositionOccupiedByObstacle(Vector3 pos)
    {
        foreach (GameObject obstacle in _obstacles)
        {
            if (Vector3.Distance(obstacle.transform.position, pos) < 1f)
            {
                return true;
            }
        }
        return false;
    }
    
    bool IsPositionOccupiedByBeehive(Vector3 pos)
    {
        foreach (Beehive beehive in _beehives)
        {
            if (Vector3.Distance(beehive.transform.position, pos) < 1f)
            {
                return true;
            }
        }
        return false;
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
            
            if (IsPositionOccupiedByFlower(pos) || IsPositionOccupiedByObstacle(pos) || IsPositionOccupiedByBeehive(pos))
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
