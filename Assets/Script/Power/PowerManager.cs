using UnityEngine;
using UnityEngine.InputSystem;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private PowerEventChannel _powerEventChannel;
    [SerializeField] private PowerLibrary _powerLibrary;

    [SerializeField] private PowerType _activePowerType = PowerType.None;
    private PowerBase _activePower;

    private void OnEnable() => _powerEventChannel.OnPowerSelected += HandlePowerSelected;
    private void OnDisable() => _powerEventChannel.OnPowerSelected -= HandlePowerSelected;

    private void Update()
    {
        if (_activePower != null && _activePower._usedPower._utilisationMethod.IsContinuous)
        {
            _activePower.UpdateContinuous();
        }
    }

    public void UsePower(InputAction.CallbackContext context)
    {
        if (_activePower == null || _activePower._usedPower._utilisationMethod.IsContinuous)
            return;

        if (context.started) _activePower.StartUse();
    }

    private void HandlePowerSelected(PowerType newPower)
    {
        if (_activePowerType == newPower)
        {
            _activePower = null;
            _activePowerType = PowerType.None;
            return;
        }
        
        _activePower = null;

        if (newPower == PowerType.None) return;
        
        Power powerData = _powerLibrary.GetPower(newPower);
        if (powerData == null) return;

        _activePowerType = newPower;
        _activePower = new PowerBase(powerData);
        
        if (_activePower._usedPower._utilisationMethod.IsContinuous)
            _activePower.StartUse();
    }
}