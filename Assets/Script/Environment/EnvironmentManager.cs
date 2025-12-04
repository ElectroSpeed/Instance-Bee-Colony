using System;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Environment Values")]
    [Range(0, 100)] public float _sunlight = 50f;
    [Range(0, 100)] public float _humidity = 50f;
    [Range(-10, 40)] public float _temperature = 20f;

    [Header("Stabilization Settings")]
    [SerializeField] private bool _autoStabilize = true;
    [SerializeField] private float _stabilizeSpeed = 1f;
    [SerializeField] private float _updateInterval = 0.5f;

    private float _updateTimer;
    public event Action<float> OnSunlightChanged;
    public event Action<float> OnHumidityChanged;

    private const float TargetValue = 50f;

    private void Update()
    {
        UpdateTimer();

        if (_autoStabilize)
            StabilizeEnvironment();
    }

    private void UpdateTimer()
    {
        _updateTimer += Time.deltaTime;

        if (_updateTimer < _updateInterval)
        {
            return;
        }
            
        _updateTimer = 0f;
    }

    private void StabilizeEnvironment()
    {
        bool changed = false;

        float newSun = Mathf.MoveTowards(_sunlight, TargetValue, _stabilizeSpeed);
        if (newSun != _sunlight)
        {
            _sunlight = newSun;
            OnSunlightChanged?.Invoke(_sunlight);
            changed = true;
        }

        float newHum = Mathf.MoveTowards(_humidity, TargetValue, _stabilizeSpeed);
        if (newHum != _humidity)
        {
            _humidity = newHum;
            OnHumidityChanged?.Invoke(_humidity);
            changed = true;
        }

        if (changed)
        {
            // Debug.Log("Environment stabilized");
        }
    }

    public void SetSunlight(float value)
    {
        _sunlight = Mathf.Clamp(value, 0f, 100f);
        OnSunlightChanged?.Invoke(_sunlight);
    }

    public void SetHumidity(float value)
    {
        _humidity = Mathf.Clamp(value, 0f, 100f);
        OnHumidityChanged?.Invoke(_humidity);
    }

    public void AddSunlight(float delta)
    {
        SetSunlight(_sunlight + delta);
    }

    public void AddHumidity(float delta)
    {
        SetHumidity(_humidity + delta);
    }
}
