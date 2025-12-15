using System;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentalUI : MonoBehaviour
{

    [Header("Scripts References")]
    [SerializeField] private EnvironmentManager _environmentManager;
    
    [Header("UI Elements")]
    [SerializeField] private Slider _sunlightSlider;
    [SerializeField] private Slider _humiditySlider;
    [SerializeField] private Slider _temperatureSlider;

    private void Update() {
        float sunlight, humidity, temperature;
        _environmentManager.GetEnvironmentValues(out sunlight, out humidity, out temperature);

        _sunlightSlider.value = sunlight;
        _humiditySlider.value = humidity;
        _temperatureSlider.value = temperature;
    }
}