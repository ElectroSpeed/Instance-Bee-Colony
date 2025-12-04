using System;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Environment Values")]
    [Range(0, 100)] public float _sunlight;
    [Range(0, 100)] public float _humidity;
    [Range(-40, 40)] public float _temperature;

    [Header("Stabilization Settings")]
    [SerializeField] private float _stabilizeSpeed;
    [SerializeField] private float _updateInterval;

    private float _updateTimer;
    public event Action<float> OnSunlightChanged;
    public event Action<float> OnHumidityChanged;
    public event Action<float> OnTemperatureChanged;

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _updateTimer += Time.deltaTime;

        if (_updateTimer < _updateInterval)
        {
            return;
        }
            
        _updateTimer = 0f;
        
        StabilizeEnvironment();
    }

    private void StabilizeEnvironment()
    {
        float target = 50f;

        _sunlight = Mathf.MoveTowards(_sunlight, target, _stabilizeSpeed);
        _humidity = Mathf.MoveTowards(_humidity, target, _stabilizeSpeed);

        OnSunlightChanged?.Invoke(_sunlight);
        OnHumidityChanged?.Invoke(_humidity);
        OnTemperatureChanged?.Invoke(_temperature);
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
}
