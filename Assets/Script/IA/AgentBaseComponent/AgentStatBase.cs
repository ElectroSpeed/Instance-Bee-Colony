using UnityEngine;

[System.Serializable]
public class AgentBase
{
    public Hunger _hunger;
    public Tiredness _tiredness; 

}

[System.Serializable]
public struct Hunger
{
    public float _minHungerValue;
    public float _maxHungerValue;
    public float _currentHungerValue;

    public Hunger(float maxHunger, float minHunger, float currentHunger = 0f)
    {
        _maxHungerValue = maxHunger;
        _minHungerValue = minHunger;
        _currentHungerValue = currentHunger;
    }
}

[System.Serializable]
public struct Tiredness
{
    public float _minTirednessValue;
    public float _maxTirednessValue;
    public float _currentTirednessValue;

    public Tiredness(float minTiredness, float maxTiredness, float currentTiredness = 0f)
    {
        _minTirednessValue = minTiredness;
        _maxTirednessValue = maxTiredness;
        _currentTirednessValue = currentTiredness;
    }
}

[CreateAssetMenu(menuName = "GodGameSO/AgentIA/StatBase", order = 1)]
public class AgentStatData : ScriptableObject
{
    [Header("Hunger Stats")]
    public float _minHungerValue;
    public float _maxHungerValue;

    [Header("Tiredness Stats")]
    public float _minTirednessValue;
    public float _maxTirednessValue;
}