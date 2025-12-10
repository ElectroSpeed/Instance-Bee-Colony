using UnityEngine;

[System.Serializable]
public struct Hunger
{
    public float _minHungerValue;
    public float _maxHungerValue;
    public float _currentHungerValue;

    public Hunger(float minHunger, float maxHunger, float currentHunger = 100f)
    {
        _minHungerValue = minHunger;
        _maxHungerValue = maxHunger;
        _currentHungerValue = currentHunger;
    }
}

[System.Serializable]
public struct Tiredness
{
    public float _minTirednessValue;
    public float _maxTirednessValue;
    public float _currentTirednessValue;

    public Tiredness(float minTiredness, float maxTiredness, float currentTiredness = 100f)
    {
        _minTirednessValue = minTiredness;
        _maxTirednessValue = maxTiredness;
        _currentTirednessValue = currentTiredness;
    }
}

[System.Serializable]
public class AgentBase
{
    public Hunger _hunger;
    public Tiredness _tiredness;
}
