using UnityEngine;
using UnityEngine.UI;

public class EnvironmentUI : MonoBehaviour
{
    [SerializeField] private Slider _sunlightSlider;
    [SerializeField] private Slider _humiditySlider;
    [SerializeField] private Slider _temperatureSlider;

    [SerializeField] private EnvironmentManager _environment;

    private void Start()
    {
        _sunlightSlider.value = _environment._sunlight;
        _humiditySlider.value = _environment._humidity;
        _temperatureSlider.value = _environment._temperature;

        _environment.OnSunlightChanged += value => _sunlightSlider.value = value;
        _environment.OnHumidityChanged += value => _humiditySlider.value = value;
        _environment.OnTemperatureChanged += value => _temperatureSlider.value = value;
    }
}