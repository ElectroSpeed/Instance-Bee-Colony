using UnityEngine;

[System.Serializable]
public class Hunger
{
    public float _minHungerValue;
    public float _maxHungerValue;
    private float _currentHungerValue;
    public event System.Action<float> OnValueChanged;

    public float Current
    {
        get => _currentHungerValue;
        set
        {
            _currentHungerValue = Mathf.Clamp(value, _minHungerValue, _maxHungerValue);
            OnValueChanged?.Invoke(_currentHungerValue);
        }
    }

    public Hunger(float minHunger, float maxHunger, float currentHunger = 50f)
    {
        _minHungerValue = minHunger;
        _maxHungerValue = maxHunger;
        _currentHungerValue = currentHunger;
    }
}

[System.Serializable]
    public class Tiredness
    {
        public float _minTirednessValue;
        public float _maxTirednessValue;
        private float _currentTirednessValue;
        public event System.Action<float> OnValueChanged;

    public float Current
        {
            get => _currentTirednessValue;
            set
            {
                _currentTirednessValue = Mathf.Clamp(value, _minTirednessValue, _maxTirednessValue);
                OnValueChanged?.Invoke(_currentTirednessValue);
            }
        }

        public Tiredness(float minTiredness, float maxTiredness, float currentTiredness = 50f)
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