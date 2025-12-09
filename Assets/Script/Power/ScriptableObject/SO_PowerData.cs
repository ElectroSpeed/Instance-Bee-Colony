using UnityEngine;

[CreateAssetMenu(menuName = "GodGame/PowerData")]
public class SO_PowerData : ScriptableObject
{
    [SerializeField] private string _powerName;
    [SerializeField] [Min(0)] private float _cooldown;

    [SerializeField] private SO_TargetingBehaviour _targetingPrefab;
    [SerializeField] private SO_EffectBehaviour[] _effectPrefabs;

    public string PowerName => _powerName;
    public float Cooldown => _cooldown;
    public SO_TargetingBehaviour TargetingPrefab => _targetingPrefab;
    public SO_EffectBehaviour[] EffectPrefabs => _effectPrefabs;
}