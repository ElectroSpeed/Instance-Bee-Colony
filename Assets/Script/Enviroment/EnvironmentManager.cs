using UnityEngine;
using System;

public class EnvironmentManager : MonoBehaviour
{
    [Range(0, 100)] public float _sunlight;
    [Range(0, 100)] public float _humidity;
    [Range(-40, 40)] public float _temperature;

    public event Action<float> OnSunlightChanged;
    public event Action<float> OnHumidityChanged;
    public event Action<float> OnTemperatureChanged;

    [Header("Stabilisation Settings")]
    [SerializeField] private float _stabilizeSpeed = 1f;
    [SerializeField] private float _updateInterval = 0.5f;

    private float _updateTimer;

    private void Update()
    {
        _updateTimer += Time.deltaTime;
        
        if (_updateTimer < _updateInterval)
            return;
        
        _updateTimer = 0f;
        
        if (!IsWeatherPowerActive())
        {
            StabilizeEnvironment();
        }
    }

    private bool IsWeatherPowerActive()
    {
        if (PowerBase.ActivePower == null)
            return false;

        //return PowerBase.ActivePower is RainPower || PowerBase.ActivePower.GetType().Name == "SunPower";
        return true;
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

    public void SetTemperature(float value)
    {
        _temperature = Mathf.Clamp(value, -40f, 40f);
        OnTemperatureChanged?.Invoke(_temperature);

    }
}