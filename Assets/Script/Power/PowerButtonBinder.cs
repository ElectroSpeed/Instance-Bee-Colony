using UnityEngine;
using UnityEngine.UI;

public class PowerButtonBinder : MonoBehaviour
{
    [SerializeField] private PowerEventChannel _powerEventChannel;
    [SerializeField] private PowerType _powerType;
    private Button _button;

    private void Awake()
    {
        if (!_button)
            _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(NotifyPowerSelected);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(NotifyPowerSelected);
    }

    private void NotifyPowerSelected()
    {
        _powerEventChannel.OnPowerSelected?.Invoke(_powerType);
    }
}