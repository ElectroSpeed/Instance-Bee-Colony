using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private SO_FlowerData _data;
    
    private EnvironmentManager _environment;

    private float _growthProgress;
    private float _timer;
    private float _elapsedLifeTime;
    private float _regenTimer;

    private bool _isGrowing = false;
    private bool _canBePollinated = false;
    private bool _isGrowed = false;
    private bool _hasPollen = false;

    private Transform _model;

    private void Start()
    {
        _environment = FindObjectOfType<EnvironmentManager>();
        if (_data.FlowerPrefab != null)
        {
            _model = Instantiate(_data.FlowerPrefab, transform).transform;
            _model.localScale = Vector3.zero;
        }
    }

    private void Update()
    {
        if (_isGrowed)
        {
            PassingLife();
            return;
        }

        if (!CanGrow())
        {
            _isGrowing = false;
            return;
        }

        Grow();
    }

    private void PassingLife()
    {
        _elapsedLifeTime += Time.deltaTime;

        if (_elapsedLifeTime >= _data.LifeDuration)
        {
            Destroy(gameObject);
        }
    }

    private bool CanGrow()
    {
        return _environment._humidity >= _data.MinHumidity && _environment._humidity <= _data.MaxHumidity &&
               _environment._sunlight >= _data.MinSunlight && _environment._sunlight <= _data.MaxSunlight &&
               _environment._temperature >= _data.MinTemperature && _environment._temperature <= _data.MaxTemperature;
    }

    private void Grow()
    {
        _isGrowing = true;
        _timer += Time.deltaTime;
        float flowerProgress = Mathf.Clamp01(_timer / _data.GrowthDuration);
        _growthProgress = _data.GrowthCurve.Evaluate(flowerProgress);

        if (_model != null)
            _model.localScale = Vector3.one * _growthProgress;

        if (_growthProgress >= 1)
        {
            _isGrowed = true;
            _canBePollinated = true;
            _hasPollen = true;
        }
    }
    
    public bool ContainsPollen() => _canBePollinated && _hasPollen;
    
    public int GetPollen()
    {
        if (!_hasPollen)
            return 0;

        int amount = _data.PollenAmount;
        _hasPollen = false;
        _canBePollinated = false;
        _regenTimer = 0f;

        return amount;
    }

    private void OnDrawGizmos()
    {
        if (_isGrowed)
        {
            Gizmos.color = _hasPollen ? Color.yellow : Color.gray;
            Gizmos.DrawSphere(transform.position + Vector3.up * 0.5f, 0.15f);
        }
    }
}
