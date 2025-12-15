using System.Collections.Generic;
using UnityEngine;

public abstract class PowerBase : MonoBehaviour, IPower
{
    [HideInInspector] public bool _usePower = false;

    [SerializeField] protected SO_PowerData _data;
    protected float _lastActivationTime;
    
    private static PowerBase _activePower;

    public static PowerBase ActivePower => _activePower;

    public virtual void SwitchActivationPower()
    {
        if (!_usePower)
        {
            if (_activePower != null && _activePower != this)
            {
                _activePower.ForceDeactivate();
            }
            
            _usePower = true;
            _activePower = this;
        }
        else
        {
            ForceDeactivate();
        }
    }
    
    private void ForceDeactivate()
    {
        _usePower = false;
        if (_activePower == this)
        {
            _activePower = null;
        }
    }

    public virtual bool CanActivatePower()
    {
        return Time.time >= _lastActivationTime + _data.Cooldown;
    }

    public virtual void ActivatePower()
    {
        if (!_usePower)
        {
            return;
        }

        if (!CanActivatePower()) return;

        _lastActivationTime = Time.time;

        if (_data.EffectPrefabs == null || _data.EffectPrefabs.Length == 0)
        {
            return;
        }

        IEnumerable<ITarget> targets = null;

        if (_data.TargetingPrefab != null)
        {
            targets = _data.TargetingPrefab.GetTargets(transform.position);
        }

        foreach (var effect in _data.EffectPrefabs)
        {
            if (effect != null)
            {
                effect.ApplyEffect(targets);
            }
        }
    }
}