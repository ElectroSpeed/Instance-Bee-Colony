using UnityEngine;

public class BeeAgent : Agent
{

    [Header("Agent need Parameters")]
    [SerializeField] private AgentStatsData _statsData;

    [Header("Decay Parameters")]
    [SerializeField] private float _hungerDecayAmount = 5f;
    [SerializeField] private float _tirednessDecayAmount = 5f;

    [Header("Hive Detection")]
    [SerializeField] private float _hiveDetectionRadius = 100f;

    [Header("Lifetime")]
    [SerializeField] private float _lifeTime = 180f;
    private float _lifeTimer;

    private float _statReduceTimer = 0f;
    private float _statReduceInterval = 5f;
    private bool _isDying = false;

    public override void Initialize()
    {
        _bb = new Blackboard();
        _bb.AddValue("AgentTransform", this.transform);

        Beehive closestHive = FindClosestHiveByTag();
        if (closestHive == null)
        {
            Debug.LogError("Bee spawned without Beehive in scene");
            return;
        }

        _bb.AddValue("Hive", closestHive);
        _bb.AddValue("HiveTransform", closestHive.transform);
        _bb.AddValue("ExplorationRadius", closestHive.explorationRadius);

        closestHive.RegisterBee();

        _bb.AddValue("Hunger", new Hunger(
            _statsData._minHungerValue,
            _statsData._maxHungerValue,
            _statsData._currentHungerValue
        ));

        _bb.AddValue("Tiredness", new Tiredness(
            _statsData._minTirednessValue,
            _statsData._maxTirednessValue,
            _statsData._currentTirednessValue
        ));

        _bb.AddValue("Health", new Health(
            _statsData._minHealthValue,
            _statsData._maxHealthValue,
            _statsData._currentHealthValue
        ));

        _bb.AddValue("TargetFlower", null);
        _bb.AddValue("CollectedPollen", 0);
        _bb.AddValue("TargetHornet", null);
        _bb.AddValue("IsUnderAttack", false);
    }


    private void OnDestroy()
    {
        if (_isDying) return;

        Beehive hive = _bb?.GetValue("Hive") as Beehive;
        if (hive != null)
        {
            hive.UnregisterBee();
        }
    }

    protected override void Update()
    {
        base.Update();

        _statReduceTimer += Time.deltaTime;
        if (_statReduceTimer >= _statReduceInterval)
        {
            _statReduceTimer = 0f;

            Hunger h = (Hunger)_bb.GetValue("Hunger");
            Tiredness t = (Tiredness)_bb.GetValue("Tiredness");

            h.Current += _hungerDecayAmount;
            t.Current += _tirednessDecayAmount;

            _bb.ModifyValue("Hunger", h);
            _bb.ModifyValue("Tiredness", t);
        }

        _lifeTimer += Time.deltaTime;
        if (_lifeTimer >= _lifeTime)
        {
            Die();
            return;
        }
    }

    public void Die()
    {
        Debug.Log("Bee died");
        _isDying = true;
        Beehive hive = (Beehive)_bb.GetValue("Hive");
        if (hive != null)
        {
            hive.UnregisterBee();
        }

        HornetAgent[] hornets = Object.FindObjectsByType<HornetAgent>(FindObjectsSortMode.None);

        foreach (HornetAgent hornet in hornets)
        {
            Blackboard bb = hornet.GetBlackboard();
            if (bb == null) continue;

            Transform targetBee = bb.GetValue("TargetBee") as Transform;
            if (targetBee == this.transform)
            {
                bb.ModifyValue("TargetBee", null);
            }
        }

        Destroy(gameObject);
    }
    private Beehive FindClosestHiveByTag()
    {
        GameObject[] hives = GameObject.FindGameObjectsWithTag("Beehive");

        Beehive closestHive = null;
        float minDist = float.MaxValue;

        foreach (var go in hives)
        {
            Beehive hive = go.GetComponent<Beehive>();
            if (hive == null) continue;

            float dist = Vector3.Distance(transform.position, hive.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestHive = hive;
                Debug.Log(closestHive.name);
            }
        }

        return closestHive;
    }

}
