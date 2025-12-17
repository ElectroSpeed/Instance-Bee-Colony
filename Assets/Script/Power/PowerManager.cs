using UnityEngine;
using UnityEngine.InputSystem;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private PowerEventChannel _powerEventChannel;
    [SerializeField] private PowerLibrary _powerLibrary;

    [SerializeField] private PowerType _activePowerType = PowerType.None;
    private PowerBase _activePower;
    private EnvironmentManager _environment;
    
    private float _continuousTimer = 0f;
    private float _timeToUpdate = 1f;

    private void OnEnable() => _powerEventChannel.OnPowerSelected += HandlePowerSelected;
    private void OnDisable() => _powerEventChannel.OnPowerSelected -= HandlePowerSelected;

    private void Start()
    {
        _environment = GameObject.FindWithTag("EnvironmentManager").GetComponent<EnvironmentManager>();
    }

    private void Update()
    {
        if (_activePower == null) return;
        if (!_activePower._usedPower._utilisationMethod.IsContinuous) return;

        _continuousTimer += Time.deltaTime;

        if (_continuousTimer >= _timeToUpdate)
        {
            _activePower.UpdateContinuous();
            _continuousTimer = 0f;
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
        if(_activePower != null && _activePower._usedPower._utilisationMethod.IsContinuous)
        {
                _activePower.StopUse();
        }
        

        if (_activePowerType == newPower)
        {
            _activePower = null;
            _activePowerType = PowerType.None;
            _environment._isPowerActive = false;
            return;
        }
        
        if (newPower != PowerType.Rain && newPower != PowerType.Sun)
            _environment._isPowerActive = false;
        
        _activePower = null;

        if (newPower == PowerType.None)
            return;
        
        Power powerData = _powerLibrary.GetPower(newPower);
        if (powerData == null) return;

        _activePowerType = newPower;
        _activePower = new PowerBase(powerData);


        if (_activePower._usedPower._utilisationMethod.IsContinuous)
            _activePower.StartUse();    
    }
}