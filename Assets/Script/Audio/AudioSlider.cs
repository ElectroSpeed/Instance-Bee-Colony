using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public enum SliderType { Master, Music, SFX }
    public SliderType sliderType;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        StartCoroutine(DelayedRegister());
    }

    private IEnumerator DelayedRegister()
    {
        yield return null;

        if (AudioSettings.Instance != null)
            AudioSettings.Instance.RegisterSlider(sliderType, _slider);
    }

    public void UpdateSlider()
    {
        AudioSettings.Instance.SetSliderValue(sliderType, _slider.value);
    }
}

