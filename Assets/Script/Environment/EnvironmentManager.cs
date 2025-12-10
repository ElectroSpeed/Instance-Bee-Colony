using System;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Environment Values")]
    [Range(0, 100)] public float _sunlight;
    [Range(0, 100)] public float _humidity;
    [Range(-10, 40)] public float _temperature;

    [Header("Stabilization Settings")]
    [SerializeField] private float _stabilizeSpeed;
    [SerializeField] private float _updateInterval;
    public bool isPowerActive = false;

    private float _updateTimer;
    public event Action<float> OnSunlightChanged;
    public event Action<float> OnHumidityChanged;

    private void Update()
    {
        if (!isPowerActive) UpdateTimer();
    }

    public void SetPowerActive(bool isActive)
    {
        isPowerActive = isActive;
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

    public void GetEnvironmentValues(out float sunlight, out float humidity, out float temperature)
    {
        sunlight = _sunlight;
        humidity = _humidity;
        temperature = _temperature;
    }
}
