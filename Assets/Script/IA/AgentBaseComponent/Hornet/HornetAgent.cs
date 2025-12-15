using UnityEngine;

public class HornetAgent : Agent
{
    [Header("Hornet Parameters")]
    [SerializeField] private float _maxHealth = 50f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackCooldown = 0.8f;
    [SerializeField] private float attackDamage = 10f;

    [Header("Detection Parameters")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask beeLayer;

    public override void Initialize()
    {
        _bb = new Blackboard();
        _bb.AddValue("AgentTransform", this.transform);
        _bb.AddValue("HealthHornet", _maxHealth);
        _bb.AddValue("TargetBee", null);
        _bb.AddValue("DetectionRadius", detectionRadius);
        _bb.AddValue("BeeLayer", beeLayer);
        _bb.AddValue("AttackRange", attackRange);
        _bb.AddValue("AttackCooldown", attackCooldown);
        _bb.AddValue("AttackDamage", attackDamage);
    }
}   