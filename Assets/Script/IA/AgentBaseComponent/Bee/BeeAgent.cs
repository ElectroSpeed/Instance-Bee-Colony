using UnityEngine;

public class BeeAgent : Agent
{

    [Header("Agent need Parameters")]
    [SerializeField] private AgentStatsData _statsData;
    [SerializeField] private Beehive hive;

    [Header("Decay Parameters")]
    [SerializeField] private float hungerDecayAmount = 5f;
    [SerializeField] private float tirednessDecayAmount = 5f;

    private float statTimer = 0f;
    private float statInterval = 5f;

    public override void Initialize()
    {
        _bb = new Blackboard();
        _bb.AddValue("AgentTransform", this.transform);

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

        _bb.AddValue("TargetFlower", null);
        _bb.AddValue("CollectedPollen", 0);
        _bb.AddValue("Hive", hive);
    }



    protected override void Update()
    {
        base.Update();

        statTimer += Time.deltaTime;
        if (statTimer >= statInterval)
        {
            statTimer = 0f;

            Hunger h = (Hunger)_bb.GetValue("Hunger");
            Tiredness t = (Tiredness)_bb.GetValue("Tiredness");

            h.Current += hungerDecayAmount;
            t.Current += tirednessDecayAmount;

            _bb.ModifyValue("Hunger", h);
            _bb.ModifyValue("Tiredness", t);
        }
    }
}
