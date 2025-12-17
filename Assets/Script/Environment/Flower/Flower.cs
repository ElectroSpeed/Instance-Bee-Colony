using System;
using System.Collections;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private SO_FlowerData _data;

    private float _growthProgress;
    private float _elapsedLifeTime;
    
    private bool _canBePollinated = false;
    private bool _isGrowed = false;
    private bool _hasPollen = false;

    private Transform _model;

    [SerializeField] private Animator animator;
    private EnvironmentManager _environment;

    private void OnEnable()
    {
        Locator<Flower>.Bind(this);
    }

    private void OnDisable()
    {
        Locator<Flower>.UnBind(this);
    }
    
    private void Start()
    {
        _environment = EnvironmentManager.Instance;
        if (_data.FlowerPrefab != null)
        {
            _model = Instantiate(_data.FlowerPrefab, transform).transform;
            _model.localScale = Vector3.zero;
        }

        animator.speed = 0f;
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
            animator.speed = 0f;
            return;
        }

        Grow();
    }

    private void PassingLife()
    {
        _elapsedLifeTime += Time.deltaTime;
        
        if (_elapsedLifeTime >= _data.LifeDuration)
        {
            animator.speed = 1f;
            _growthProgress = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (_growthProgress >= 1f)
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
        _growthProgress = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (_model != null)
        {
            animator.speed = 1f;

        }       

        if (_growthProgress >= 0.5f)
        {
            animator.speed = 0f;
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


