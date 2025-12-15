using UnityEngine;

[CreateAssetMenu(menuName = "GodGameSO/AgentIA/StatBase", order = 1)]
public class AgentStatsData : ScriptableObject
{
    [Header("Hunger Stats")]
    public float _minHungerValue = 0f;
    public float _maxHungerValue = 100f;
    public float _currentHungerValue = 50f;

    [Header("Tiredness Stats")]
    public float _minTirednessValue = 0f;
    public float _maxTirednessValue = 100f;
    public float _currentTirednessValue = 50f;

    [Header("Health Stats")]
    public float _minHealthValue = 0f;
    public float _maxHealthValue = 100f;
    public float _currentHealthValue = 100f;
}