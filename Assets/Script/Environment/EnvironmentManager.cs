using System;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Environment Values")]
    [Range(0, 100)] public float Sunlight = 50f;
    [Range(0, 100)] public float Humidity = 50f;

    [Header("Stabilization Settings")]
    [SerializeField] private bool autoStabilize = true;
    [SerializeField] private float stabilizeSpeed = 1f;
    [SerializeField] private float updateInterval = 0.5f;

    private float updateTimer;

    // Events (create a new action when you have a new environment change, EX : you create a pollution variable for flower, you create a new action for this)
    public event Action<float> OnSunlightChanged;
    public event Action<float> OnHumidityChanged;

    private const float TargetValue = 50f;

    private void Update()
    {
        updateTimer += Time.deltaTime;

        if (updateTimer < updateInterval)
            return;

        updateTimer = 0f;

        if (autoStabilize)
            StabilizeEnvironment();
    }

    private void StabilizeEnvironment()
    {
        bool changed = false;

        float newSun = Mathf.MoveTowards(Sunlight, TargetValue, stabilizeSpeed);
        if (newSun != Sunlight)
        {
            Sunlight = newSun;
            OnSunlightChanged?.Invoke(Sunlight);
            changed = true;
        }

        float newHum = Mathf.MoveTowards(Humidity, TargetValue, stabilizeSpeed);
        if (newHum != Humidity)
        {
            Humidity = newHum;
            OnHumidityChanged?.Invoke(Humidity);
            changed = true;
        }

        if (changed)
        {
            // Debug.Log("Environment stabilized");
        }
    }

    public void SetSunlight(float value)
    {
        Sunlight = Mathf.Clamp(value, 0f, 100f);
        OnSunlightChanged?.Invoke(Sunlight);
    }

    public void SetHumidity(float value)
    {
        Humidity = Mathf.Clamp(value, 0f, 100f);
        OnHumidityChanged?.Invoke(Humidity);
    }

    public void AddSunlight(float delta)
    {
        SetSunlight(Sunlight + delta);
    }

    public void AddHumidity(float delta)
    {
        SetHumidity(Humidity + delta);
    }
}
