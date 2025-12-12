using UnityEngine;

public class HornetAgent : Agent
{
    [Header("Hornet Parameters")]
    [SerializeField] private float _maxHealth = 50f;

    [Header("Detection Parameters")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask beeLayer;

    public override void Initialize()
    {
        _bb = new Blackboard();
        _bb.AddValue("AgentTransform", this.transform);
        _bb.AddValue("Health", _maxHealth);
        _bb.AddValue("TargetBee", null);
        _bb.AddValue("DetectionRadius", detectionRadius);
        _bb.AddValue("BeeLayer", beeLayer);
    }
}   